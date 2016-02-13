using System;

namespace Rescuer.Management.WindowsService.Exceptions
{
    public class ServiceConnectionException : Exception
    {
        public ServiceConnectionException()
        {
            
        }

        public ServiceConnectionException(string message) : base(message)
        {
            
        }

        public ServiceConnectionException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}