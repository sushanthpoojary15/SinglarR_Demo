using SignalR_demo.Models;

namespace SignalR_demo.Singnal_r_services
{
    public interface ISignalRService
    {

        Task SendEmpData(Emp emp);
    }
}
