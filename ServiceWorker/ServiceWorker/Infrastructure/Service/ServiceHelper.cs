using ServiceWorker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace ServiceWorker.Infrastructure.Service
{
    interface IServiceHelper
    {
        List<ServiceModel> GetServices();
        void StartService(string name);
        void StopService(string name);
    }
    public class ServiceHelper : IServiceHelper
    {
        public List<ServiceModel> GetServices()
        {
            try
            {
                return ServiceController.GetServices().Select(x => new ServiceModel
                {
                    Account = x.MachineName,
                    DisplayName = x.DisplayName,
                    Name = x.ServiceName,
                    Status = x.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void StartService(string name)
        {
            try
            {
                var service = ServiceController.GetServices().First(x => x.ServiceName == name);
                service.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void StopService(string name)
        {
            try
            {
                var service = ServiceController.GetServices().First(x => x.ServiceName == name);
                service.Stop();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
