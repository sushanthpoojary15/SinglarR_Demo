using Microsoft.AspNetCore.SignalR;
using SignalR_demo.Models;

namespace SignalR_demo.Singnal_r_services
{
    public class SignalRService:ISignalRService
    {
        public readonly IHubContext<EmpHub> _hubContext;

        public SignalRService(IHubContext<EmpHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendEmpData(Emp emp)
        {
            await _hubContext.Clients.All.SendAsync("EmpAddData",emp);
        }
    }
}
