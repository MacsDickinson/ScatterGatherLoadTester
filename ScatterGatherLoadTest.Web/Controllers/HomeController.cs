using ScatterGatherLoadTest.Web.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ScatterGatherLoadTest.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoadTest(LoadTestPostModel model)
        {
            var request = new LoadTestRequest
            {
                Domain = model.Domain,
                Resource = model.Resource,
                Method = RestSharp.Method.GET,
                RequestMultiplyer = model.Requests
            };
            
            var response = ServiceBus.Instance.Aggregate<LoadTestRequest, LoadTestResponse, LoadTestAggregation>(request);
            
            return View(await response);
        }
    }
}