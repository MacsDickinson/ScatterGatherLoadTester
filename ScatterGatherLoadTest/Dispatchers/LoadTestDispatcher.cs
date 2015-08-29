using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest.Dispatchers
{
    public class LoadTestDispatcher : IServiceDispatcher<LoadTestRequest, LoadTestResponse>
    {
        public IEnumerable<Task<LoadTestResponse>> Dispatch(IService<LoadTestRequest, LoadTestResponse> service, LoadTestRequest request)
        {
            var responses = new List<Task<LoadTestResponse>>();

            for (int i = 0; i < request.RequestMultiplyer; i++)
            {
                var response = service.Execute(request);
                responses.Add(response);
            }

            return responses;
        }
    }
}
