using System;

namespace ScatterGatherLoadTest
{
    public class LoadTestResponse : IServiceResponse
    {
        public bool Success { get; set; }
        public long Milliseconds { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}