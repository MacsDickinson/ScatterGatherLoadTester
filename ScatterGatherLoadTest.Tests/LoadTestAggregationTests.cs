using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest.Tests
{
    [TestFixture]
    public class LoadTestAggregationTests
    {
        [Test]
        public async Task AggregateShouldCallServices()
        {
            // Arrange
            var request = new LoadTestRequest
            {
                Domain = "http://www.google.com",
                Resource = "",
                Method = RestSharp.Method.GET ,
                RequestMultiplyer = 15
            };

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            LoadTestAggregation response = await ServiceBus.Instance.Aggregate<LoadTestRequest, LoadTestResponse, LoadTestAggregation>(request);
            stopwatch.Stop();
            // Assert
            response.SuccessfulRequests.ShouldBeEquivalentTo(15);
            response.AverageMillisecondsPerRequest.Should().BeGreaterThan(0);
            response.TotalCombinedMilliseconds.Should().BeGreaterThan(stopwatch.ElapsedMilliseconds);
        }
    }
}
