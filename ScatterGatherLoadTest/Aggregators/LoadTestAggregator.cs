using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest.Aggregators
{
    public class LoadTestAggregator : IServiceAggregation<LoadTestAggregation, LoadTestResponse>
    {
        public async Task<LoadTestAggregation> Aggregate(IEnumerable<Task<LoadTestResponse>> responses)
        {
            return await Task.WhenAll(responses)
                .ContinueWith(task =>
                {
                    var totalCombinedTicks = task.Result.Sum(x => x.Milliseconds);
                    var successfulRequests = task.Result.Count(x => x.Success);
                    var averageTicksPerRequest = task.Result.Count(x => x.Success) == 0 ? 0 : task.Result.Where(x => x.Success).Sum(x => x.Milliseconds) / task.Result.Count(x => x.Success);

                    return new LoadTestAggregation {
                        TotalCombinedMilliseconds = totalCombinedTicks,
                        SuccessfulRequests = successfulRequests,
                        AverageMillisecondsPerRequest = averageTicksPerRequest,
                        Results = task.Result
                    };
                });
        }
    }
}
