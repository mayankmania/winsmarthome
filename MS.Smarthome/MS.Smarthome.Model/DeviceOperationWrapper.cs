using System.Collections.Generic;

namespace MS.Smarthome.Model
{
    public class DeviceOperationWrapper
    {
        public DeviceOperationWrapper()
        {
            Operation = new Operation();
            Devices = new List<Device>();
        }

        public Operation Operation { get; set; }
        public List<Device> Devices { get; set; }
    }
}
