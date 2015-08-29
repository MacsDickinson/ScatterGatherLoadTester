using System;
using System.Collections.Generic;
using RestSharp;

namespace ScatterGatherLoadTest
{
    public class LoadTestRequest : IServiceRequest
    {
        public string Domain { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string Resource { get; set; }
        public Method Method { get; set; }
        public int RequestMultiplyer { get; set; }

        public LoadTestRequestAthentication Authentication { get; set; }

        public LoadTestRequest()
        {
            Authentication = new LoadTestRequestAthentication();
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
        }
    }
}
