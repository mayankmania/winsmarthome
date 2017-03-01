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
            Groups.Add(Context.ConnectionId, "1");
            return base.OnConnected();
        }

        public void OperationProcessed(DeviceOperationWrapper deviceOperationWrapper)
        {
            File.WriteAllText("File.txt","OperationProcessed Invoked");
            switch (deviceOperationWrapper.Operation.OperationType)
            {
                case OperationType.GET:
                    Clients.Group(deviceOperationWrapper.Operation.RaspId).deviceListFetched(deviceOperationWrapper.Devices);
                    break;
                case OperationType.POST:
                    Clients.Group(deviceOperationWrapper.Operation.RaspId).onDeviceUpdated(deviceOperationWrapper.Devices);
                    break;
                default:
                    break;
            }
        }

        public void GetDevices()
        {
            var operation = new Operation();
            operation.RaspId = "1";
            operation.OperationType = OperationType.GET;
            Clients.Group(operation.RaspId).processRecord(operation);
        }

        public void PerformOperation(Operation operation)
        {
            operation.RaspId = "1";
            operation.OperationType = OperationType.POST;
            Clients.Group(operation.RaspId).processRecord(operation);
        }
    }
}