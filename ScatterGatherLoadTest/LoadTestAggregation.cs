using System.Collections.Generic;

namespace ScatterGatherLoadTest
{
    public class LoadTestAggregation
    {
        public long TotalCombinedMilliseconds { get; set; }
        public long AverageMillisecondsPerRequest { get; set; }
        public int SuccessfulRequests { get; set; }
        public IEnumerable<LoadTestResponse> Results { get; set; }
    }
}
