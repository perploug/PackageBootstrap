using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackageBootstrap.Configuration;
using PackageBootstrap.Attributes;

namespace PackageBootstrap.ProviderModel
{
    [Serializable]
    public abstract class ProviderTypeBase
    {
        #region Properties (4)

        public string Description { get; set; }

        public Guid Id { get; set; }

        //index *can* determine in what order a provider is loaded, normally this is not needed to set.
        //the higher the index, the quicker it's loaded
        public int Index { get; set; }
        public string Name { get; set; }
        
        public T GetProvider<T>()
        {
            return default(T);
        }
        
        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        public ProviderBaseConfiguration Configuration { get; private set; }
        public void LoadConfiguration(ProviderBaseConfiguration configuration)
        {
            foreach (var keyval in this.AvailableSettings())
            {
                string value = string.Empty;

                if (configuration.ContainsKey(keyval.Key))
                    value = configuration[keyval.Key];

                this.GetType().InvokeMember(keyval.Key, System.Reflection.BindingFlags.SetProperty, null, this, new object[] { value });
            }
        }

        private Dictionary<string, Setting> m_settings = null;
        public virtual Dictionary<string,Setting> AvailableSettings()
        {
            if (m_settings == null)
            {

                Dictionary<string, Setting> s = new Dictionary<string, Setting>();
                foreach (System.Reflection.PropertyInfo p in this.GetType().GetProperties())
                {
                    object[] o = p.GetCustomAttributes(true);

                    if (o.Length > 0 && o[0].GetType() == typeof(Attributes.Setting))
                        s.Add(p.Name, (Setting)o[0]);
                }

                m_settings = s;
            }

            return m_settings;
        }

        private Dictionary<string, System.Reflection.MethodInfo> m_prevalues = null;
        public virtual Dictionary<string, System.Reflection.MethodInfo> AvailableSettingPrevalues()
        {
            if (m_prevalues == null)
            {

                Dictionary<string, System.Reflection.MethodInfo> s = new Dictionary<string, System.Reflection.MethodInfo>();
                foreach (System.Reflection.MethodInfo m in this.GetType().GetMethods())
                {
                    object[] o = m.GetCustomAttributes(true);
                    
                    if (o.Length > 0 && o[0].GetType() == typeof(Attributes.SettingPrevalues))
                    {
                        if (m.ReturnType == typeof(Dictionary<string, string>))
                        {
                            var pr = (SettingPrevalues)o[0];
                            s.Add(pr.GetName(), m);
                        }
                    }
                }

                m_prevalues = s;
            }

            return m_prevalues;
        }


        public virtual Dictionary<string, string> GetPrevalues(Setting setting)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>(); 
            
            if (AvailableSettingPrevalues().ContainsKey(setting.GetName()))
            {
                retval = AvailableSettingPrevalues()[setting.GetName()].Invoke(this, null) as Dictionary<string, string>;
            }
            
            return retval;
        }

        #endregion Methods
    }
}
