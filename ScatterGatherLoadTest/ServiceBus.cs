using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest
{
    public class ServiceBus
    {

        public static readonly ServiceBus Instance = new ServiceBus();

        public async Task<TAggregation> Aggregate<TServiceRequest, TServiceResponse, TAggregation>(TServiceRequest request)
            where TServiceRequest : IServiceRequest
            where TServiceResponse : IServiceResponse
        {
            var dispatcherType = GetTypesImplementing(typeof(IServiceDispatcher<TServiceRequest, TServiceResponse>)).Single();
            var aggregatorType = GetTypesImplementing(typeof(IServiceAggregation<TAggregation, TServiceResponse>)).Single();
            var dispatcher = (IServiceDispatcher<TServiceRequest, TServiceResponse>)Activator.CreateInstance(dispatcherType);
            var aggregator = (IServiceAggregation<TAggregation, TServiceResponse>)Activator.CreateInstance(aggregatorType);

            var serviceTasks = GetTypesImplementing(typeof(IService<TServiceRequest, TServiceResponse>))
                .Select(t => (IService<TServiceRequest, TServiceResponse>)Activator.CreateInstance(t))
                .SelectMany(s => dispatcher.Dispatch(s, request));

            return await aggregator.Aggregate(serviceTasks);
        }

        private IEnumerable<Type> GetTypesImplementing(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(t => t.GetInterfaces().Contains(type)));
        }
    }
}
