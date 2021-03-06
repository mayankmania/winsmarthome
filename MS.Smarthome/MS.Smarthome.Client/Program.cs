﻿using Microsoft.AspNet.SignalR.Client;
using MS.Smarthome.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace MS.Smarthome.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var macAddr =
    (
        from nic in NetworkInterface.GetAllNetworkInterfaces()
        where nic.OperationalStatus == OperationalStatus.Up
        select nic.GetPhysicalAddress().ToString()
    ).FirstOrDefault();

            var hubConnection = new HubConnection("http://localhost:49348/", "raspId=" + macAddr);
            IHubProxy rpihubProxy = hubConnection.CreateHubProxy("rpiHub");
            rpihubProxy.On<Operation>("ProcessRecord", operation =>
            {
                var deviceOperation = PerformOperation(operation);
                rpihubProxy.Invoke<DeviceOperationWrapper>("OperationProcessed", deviceOperation);
            });
            hubConnection.Start().Wait();
            Console.ReadLine();
        }

        public static DeviceOperationWrapper PerformOperation(Operation operation)
        {
            List<Device> deviceDetails = new List<Device>();

            switch (operation.OperationType)
            {
                case OperationType.GET:
                    deviceDetails.AddRange(GetRegisteredDevices());
                    break;
                case OperationType.POST:
                    deviceDetails.AddRange(GetDeviceDetails(operation.DeviceId));
                    break;
                default:
                    break;
            }

            return new DeviceOperationWrapper { Devices = deviceDetails, Operation = operation };
        }

        public static List<Device> GetRegisteredDevices()
        {
            var devices = new List<Device>();
            devices.Add(new Device { DeviceId = "15", Description = "fan" });
            devices.Add(new Device { DeviceId = "16", Description = "bulb" });
            devices.Add(new Device { DeviceId = "18", Description = "washer" });
            devices.Add(new Device { DeviceId = "19", Description = "tv" });
            foreach (var device in devices)
            {
                device.Status = false;
            }
            return devices;
        }

        public static List<Device> GetDeviceDetails(string deviceId)
        {
            return GetRegisteredDevices().Where(c => c.DeviceId == deviceId).Select(c => new Device { Description = c.Description, Status = true, DeviceId = c.DeviceId }).ToList();
        }
    }
}