using SharedProject1;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace WorkerService1.Services
{
    public class RemoteService
    {
        private readonly IFunctionService service;
        private readonly string machineName;

        public RemoteService(IFunctionService service)
        {
            this.service = service;
            this.machineName = GetFQDN();
        }

        public async Task SendHeartbeat()
        {
            var message = new SomeModel
            {
                MachineName = machineName,
                UpSince = DateTime.Now
            };

            await service.RecordHeartbeat(message);
        }


        private string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;

            if (!hostName.EndsWith(domainName))     // if hostname does not already include domain name
            {
                hostName += domainName;             // add the domain name part
            }

            return hostName;                        // return the fully qualified name
        }
    }
}
