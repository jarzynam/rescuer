﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;
using Continuous.WindowsService.Model.Enums;

namespace Continuous.WindowsService.Tests.TestHelpers
{
    internal static class ServiceHelper
    {
        private const int WaitInSeconds = 5;

        internal static int GetErrorControl(string serviceName)
        {
            var result = GetProperty(serviceName, "ErrorControl");

            return int.Parse(result.ToString());
        }

        internal static int GetServiceType(string serviceName)
        {
            var result = GetProperty(serviceName, "Type");

            return int.Parse(result.ToString());
        }

        internal static int GetStartMode(string serviceName)
        {
            var result = GetProperty(serviceName, "Start");

            return int.Parse(result.ToString());
        }

        internal static string GetPath(string serviceName)
        {
            return GetProperty(serviceName, "ImagePath").ToString();
        }

        internal static string GetDisplayName(string serviceName)
        {
            return GetProperty(serviceName, "DisplayName").ToString();
        }

        internal static string GetAccount(string serviceName)
        {
            return GetProperty(serviceName, "ObjectName").ToString();
        }

        internal static string GetDescription(string serviceName)
        {
            return GetProperty(serviceName, "Description")?.ToString();
        }

        internal static bool GetDelayedAutostart(string serviceName)
        {
            var isDelayedStr = GetProperty(serviceName, "DelayedAutostart")?.ToString();

            return isDelayedStr == "1";
        }

        internal static ICollection<string> GetServiceDependencies(string serviceName)
        {
            var result = GetProperties(serviceName, "DependOnService")
                .Where(p => p?.BaseObject != null)
                .Select(p => p.BaseObject.ToString())
                .ToList();

            return result;
        }

        private static PSObject GetProperty(string serviceName, string property)
        {
            return GetProperties(serviceName, property).FirstOrDefault();
        }

        private static ICollection<PSObject> GetProperties(string serviceName, string property)
        {
            var command = $@"(Get-ItemProperty 'HKLM:\SYSTEM\CurrentControlSet\Services\{serviceName}').{property}";

            return ScriptInvoker.InvokeScript(command);
        }

        internal static bool Exist(string name)
        {
            var command = $"Get-Service {name} -ErrorAction Ignore";

            var result =  ScriptInvoker.InvokeScript(command);

            return result.Any();
        }

        internal static void StartService(string name)
        {
            var service = new ServiceController(name);

            service.Start();

            service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(WaitInSeconds));
        }

        internal static void StopService(string name)
        {
            var service = new ServiceController(name);

            service.Stop();

            service.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        internal static void PauseService(string serviceName, bool waitForStatus = true)
        {
            var service = new ServiceController(serviceName);

            service.Pause();

            if (waitForStatus)
                service.WaitForStatus(ServiceControllerStatus.Paused);
        }

        internal static WindowsServiceState GetState(string serviceName)
        {
            var service = new ServiceController(serviceName);

            return (WindowsServiceState) Enum.Parse(typeof(WindowsServiceState), service.Status.ToString());
        }

        internal static WindowsServiceTestModel GetService(string name)
        {
            var model = new WindowsServiceTestModel
            {
                Name = name,
                DisplayName = GetDisplayName(name),
                ErrorControl = (WindowsServiceErrorControl) GetErrorControl(name),
                Type = (WindowsServiceType) GetServiceType(name),
                StartMode = (WindowsServiceStartMode) GetStartMode(name),
                Account = GetAccount(name),
                Path = GetPath(name),
                Description = GetDescription(name),
                ServiceDependencies = GetServiceDependencies(name)
            };

            return model;
        }
    }

    internal class WindowsServiceTestModel
    {
        internal string Name { get; set; }
        internal string DisplayName { get; set; }
        internal WindowsServiceErrorControl ErrorControl { get; set; }
        internal WindowsServiceType Type { get; set; }
        internal WindowsServiceStartMode StartMode { get; set; }
        internal string Account { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public ICollection<string> ServiceDependencies { get; set; }
    }
}