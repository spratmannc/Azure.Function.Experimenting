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
            var model = await req.ReadAsAsync<SomeModel>();

            log.LogInformation("Entry received: {machine}, {date}", model.MachineName, model.UpSince);

            return new OkResult();
        }
    }
}
