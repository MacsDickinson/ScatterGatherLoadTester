using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ScatterGatherLoadTest.Services
{
    public class SingleRequestService : IService<LoadTestRequest, LoadTestResponse>
    {
        public async Task<LoadTestResponse> Execute(LoadTestRequest request)
        {
            var response = new LoadTestResponse();

            var stopwatch = new Stopwatch();

            try
            {
                response.Start = DateTime.UtcNow;
                stopwatch.Start();
                response.Success = await MakeRequest(request) == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                response.Success = false;
            }
            finally
            {
                stopwatch.Stop();
                response.Milliseconds = stopwatch.ElapsedMilliseconds;
                response.End = DateTime.UtcNow;
            }

            return response;
        }

        private static Task<HttpStatusCode> MakeRequest(LoadTestRequest request)
        {
            var client = new RestClient(request.Domain);
            if (request.Authentication.Enabled)
            {
                client.Authenticator = new HttpBasicAuthenticator(request.Authentication.Username, request.Authentication.Password);
            }

            var restRequest = new RestRequest(request.Resource, request.Method);

            foreach (KeyValuePair<string, string> parameter in request.Parameters)
            {
                restRequest.AddParameter(parameter.Key, parameter.Value);
            }

            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                restRequest.AddHeader(header.Key, header.Value);
            }

            var taskCompletionSource = new TaskCompletionSource<HttpStatusCode>();
            client.ExecuteAsync(restRequest, (response) =>
            {
                taskCompletionSource.SetResult(response.StatusCode);
            });
            return taskCompletionSource.Task;
        }
    }
}
