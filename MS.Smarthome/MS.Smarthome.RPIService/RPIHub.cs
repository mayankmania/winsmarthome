using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MS.Smarthome.Model;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MS.Smarthome.RPIService
{
    [HubName("rpiHub")]
    public class RPIHub : Hub
    {
        public RPIHub()
        {

        }

        public override Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, Context.QueryString["raspId"]);
            return base.OnConnected();
        }

        public void OperationProcessed(DeviceOperationWrapper deviceOperationWrapper)
        {
            switch (deviceOperationWrapper.Operation.OperationType)
            {
                case OperationType.GET:
                    Clients.Group(deviceOperationWrapper.Operation.RaspId).deviceListFetched(deviceOperationWrapper.Devices);
                    break;
                case OperationType.POST:
                    Clients.Group(deviceOperationWrapper.Operation.RaspId).deviceUpdated(deviceOperationWrapper.Devices[0]);
                    break;
                default:
                    break;
            }
        }

        public void GetDevices()
        {
            var operation = new Operation()
            {
                RaspId = Context.QueryString["raspId"],
                OperationType = OperationType.GET
            };

            Clients.Group(operation.RaspId).processRecord(operation);
        }

        public void PerformOperation(Operation operation)
        {
            operation.RaspId = Context.QueryString["raspId"];
            operation.OperationType = OperationType.POST;
            Clients.Group(operation.RaspId).processRecord(operation);
        }
    }
}