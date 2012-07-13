using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackageBootstrap.ProviderModel;
using System.Web;

namespace PackageBootstrap.Configuration
{
    [Serializable]
    public class ProviderBaseConfiguration : Dictionary<string,string>
    {
        public ProviderBaseConfiguration()
        {
            
        }

        public void PopulateFromRequest(ProviderTypeBase providerType, HttpRequestBase Request)
        {
            foreach (var s in providerType.AvailableSettings())
            {
                if (Request["Setting_" + s.Key] != null)
                {
                    if (this.ContainsKey(s.Key))
                        this[s.Key] = Request["Setting_" + s.Key];
                    else
                        this.Add(s.Key, Request["Setting_" + s.Key]);
                }
            }

        }
    }
}
