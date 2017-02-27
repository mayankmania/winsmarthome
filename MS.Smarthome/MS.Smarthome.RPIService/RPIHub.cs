using Microsoft.AspNet.SignalR;
using MS.Smarthome.RPIService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MS.Smarthome.RPIService
{
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

        public void OperationProcessed(DeviceDetail deviceDetail)
        {
            switch (deviceDetail.Operation.OperationType)
            {
                case OperationType.GET:
                    Clients.Group(deviceDetail.Operation.RaspId).deviceListFetched(deviceDetail.Devices);
                    break;
                case OperationType.POST:
                    Clients.Group(deviceDetail.Operation.RaspId).onDeviceUpdated(deviceDetail.Devices);
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