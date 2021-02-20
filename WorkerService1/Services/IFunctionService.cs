using Refit;
using SharedProject1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkerService1.Services
{
    public interface IFunctionService
    {
        [Post("/api/RecordHeartbeat")]
        Task RecordHeartbeat([Body] Heartbeat heartbeat);

        [Get("/api/GetEventLogFilters")]
        Task<IEnumerable<string>> GetEventLogFilters();

        [Post("/api/Warn")]
        Task Warn([Body] Warning warning);
    }
}
