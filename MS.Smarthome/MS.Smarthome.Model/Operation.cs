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
}