using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SharedProject1;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class RemoteServices
    {
        private readonly ILogger<RemoteServices> log;

        public RemoteServices(ILogger<RemoteServices> log)
        {
            this.log = log;
        }

        [FunctionName("RecordHeartbeat")]
        public async Task<IActionResult> RecordHeartbeat(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req
        )
        {
            var model = await req.ReadAsAsync<Heartbeat>();

            log.LogWarning("Heartbeat from {machine}", model.MachineName, model.UpSince);

            return new OkResult();
        }

        [FunctionName("Warn")]
        public async Task<IActionResult> Warn(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req
        )
        {
            var model = await req.ReadAsAsync<Warning>();

            log.LogError("Warning from {machine}: '{message}'", model.MachineName, model.Message);

            return new OkResult();
        }

        [FunctionName("GetEventLogFilters")]
        public IActionResult GetEventLogFilters(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req
        )
        {
            var filters = new string[]
            {
                "One",
                "Two",
                "Three"
            };

            log.LogInformation("Filters requested");

            return new OkObjectResult(filters);
        }
    }
}
