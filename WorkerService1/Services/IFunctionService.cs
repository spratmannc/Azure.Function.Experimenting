using Refit;
using SharedProject1;
using System.Threading.Tasks;

namespace WorkerService1.Services
{
    public interface IFunctionService
    {
        [Post("/api/RecordHeartbeat")]
        Task RecordHeartbeat([Body] SomeModel model);
    }
}
