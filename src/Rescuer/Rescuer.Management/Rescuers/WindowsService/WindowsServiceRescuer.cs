using System.Linq;
using System.ServiceProcess;
using Rescuer.Management.Rescuers.WindowsService.Exceptions;
using Rescuer.Management.Rescuers.WindowsService.Shell;
using Rescuer.Management.Transit;

namespace Rescuer.Management.Rescuers.WindowsService
{
    public class WindowsServiceRescuer : IWindowsServiceRescuer
    {
        private readonly IWindowsServiceShell _serviceShell;        

        public int RescueCounter { get; private set; }

        public WindowsServiceRescuer(IWindowsServiceShell serviceShell)
        {
            _serviceShell = serviceShell;            
        }

        public HealthStatus CheckHealth()
        {
            var serviceStatus = _serviceShell.GetServiceStatus();

            return serviceStatus == ServiceControllerStatus.Running
                ? HealthStatus.Working
                : HealthStatus.Stopped;
        }

        public bool Rescue()
        {
            var startResult = _serviceShell.StartService();

            if(startResult == false)
                throw new ServiceRescueException(_serviceShell.ErrorLog.LastOrDefault());

            RescueCounter++;

            return true;
        }

        public void Connect(string serviceName)
        {
            var connectionResult = _serviceShell.ConnectToService(serviceName);

            if(!connectionResult)
                throw new ServiceConnectionException(_serviceShell.ErrorLog.LastOrDefault());
        }

        public void MonitorAndRescue()
        {
            var status = CheckHealth();

            if (status != HealthStatus.Working)
                Rescue();            
        }        

        public void Dispose()
        {
            _serviceShell.Dispose();
        }
    }
}