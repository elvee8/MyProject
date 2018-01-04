using System.Collections.Generic;

namespace FormSMSMultipleInstance
{
    public class Device
    {
        public string Port { get; set; }

        public string Status { get; set; }

        public string DeviceNumber { get; set; }

        public Device(string portNum)
        {
            Port = portNum;
        }

        //IDictionary<int, string> Status = new Dictionary<int, string>()
        //                                                    {
        //                                                        {-1, "Device Error!"},
        //                                                        {0, "Device Available"},
        //                                                        {1, "Ready To Connect."},
        //                                                        {2, "Connecting..."},
        //                                                        {3, "Connected"}
        //                                                    };
    }
}
