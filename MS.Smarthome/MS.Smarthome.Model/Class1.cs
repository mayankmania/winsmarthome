using System.Collections.Generic;

namespace MS.Smarthome.Model
{
    public enum OperationType
    {
        GET,
        POST
    }

    public class Operation
    {
        public string RaspId { get; set; }
        public string DeviceId { get; set; }
        public OperationType OperationType { get; set; }
    }

    public class DeviceDetail
    {
        public DeviceDetail()
        {
            Operation = new Operation();
            Devices = new List<Device>();
        }

        public Operation Operation { get; set; }
        public List<Device> Devices { get; set; }
    }
}
