using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest.Tests
{
    [TestFixture]
    public class ServiceBusAggregationSpecificationTests
    {
        [Test]
        public async Task AggregateShouldCallServices()
        {
            var command = new TestAggregationCommand();
            var response = await ServiceBus.Instance.Aggregate<TestAggregationRequest, TestAggregationResponse, TestAggregation>(new TestAggregationRequest { Command = command });
            Assert.AreEqual(3, response.Count);
        }
    }


    public class TestAggregationServiceAggregation : IServiceAggregation<TestAggregation, TestAggregationResponse>
    {
        public async Task<TestAggregation> Aggregate(IEnumerable<Task<TestAggregationResponse>> responses)
        {
            return await Task.WhenAll(responses)
                .ContinueWith(task =>
                {
                    return new TestAggregation { Count = task.Result.Sum(r => r.Command.Value) };
                });
        }
    }

    public class TestAggregationService : IService<TestAggregationRequest, TestAggregationResponse>
    {
        public async Task<TestAggregationResponse> Execute(TestAggregationRequest request)
        {
            return new TestAggregationResponse { Command = new TestAggregationCommand { Value = 2 } };
        }
    }

    public class TestAggregationService2 : IService<TestAggregationRequest, TestAggregationResponse>
    {
        public async Task<TestAggregationResponse> Execute(TestAggregationRequest request)
        {
            return new TestAggregationResponse { Command = request.Command };
        }
    }

    public class TestAggregationRequest : IServiceRequest
    {
        public TestAggregationCommand Command { get; set; }
    }

    public class TestAggregationResponse : IServiceResponse
    {
        public TestAggregationCommand Command { get; set; }
    }

    public class TestAggregationCommand
    {
        public int Value = 1;
    }

    public class TestAggregation
    {
        public int Count;
    }
}
