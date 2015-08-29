using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest
{
    public interface IServiceRequest
    {
    }

    public interface IServiceResponse
    {
    }

    public interface IService<in TServiceRequest, TServiceResponse>
        where TServiceRequest : IServiceRequest
        where TServiceResponse : IServiceResponse
    {
        Task<TServiceResponse> Execute(TServiceRequest request);
    }

    public interface IServiceAggregation<TAggregate, TServiceResponse>
        where TServiceResponse : IServiceResponse
    {
        Task<TAggregate> Aggregate(IEnumerable<Task<TServiceResponse>> responses);
    }

    public interface IServiceDispatcher<TServiceRequest, TServiceResponse>
        where TServiceRequest : IServiceRequest
        where TServiceResponse : IServiceResponse
    {
        IEnumerable<Task<TServiceResponse>> Dispatch(IService<TServiceRequest, TServiceResponse> service, TServiceRequest request);
    }
}
