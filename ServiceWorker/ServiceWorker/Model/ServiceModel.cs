using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWorker.Model
{
    public class ServiceModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ServiceControllerStatus Status { get; set; }
        public string Account { get; set; }

        public override int GetHashCode()
        {
            return (Name + DisplayName + Status + Account).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as ServiceModel;
            if (other == null) return false;

            return Name == other.Name && DisplayName == other.DisplayName && Status == other.Status && Account == other.Account;
        }
    }
}
