// -----------------------------------------------------------------------
// <copyright file="ProviderBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.ProviderModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using PackageBootstrap.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ProviderBase<T> : IProvider where T : ProviderTypeBase
    {
        private T type = null;
        private bool initialized = false;

        public ProviderTypeBase Type
        {
            get
            {
                if (type == null)
                    throw new Exception("Provider Type cannot be Null");

                if (!initialized)
                    Initialize(type);

                return type;
            }   
        }

        public ProviderBase()
        {

        }

        public void Initialize(object ProviderType, bool loadSettings = true)
        {
            this.type = DeepCloning.Clone<T>((T)ProviderType);
            this.TypeClassName = this.GetType().Name;
            
            if(loadSettings)
                this.type.LoadConfiguration(Configuration);

            this.initialized = true;
        }

        /*
        public ProviderBase(T type, ProviderBaseConfiguration settings)
        {
            Type = type;
            Type.LoadSettings(settings);
            TypeClassName = typeof(T).ToString();
        }*/

        private ProviderBaseConfiguration m_config = null;
        public ProviderBaseConfiguration Configuration
        {
            get
            {
                if (m_config == null)
                {
                    if (!string.IsNullOrEmpty(m_settings))
                    {
                        JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                        m_config = oSerializer.Deserialize<ProviderBaseConfiguration>(m_settings);
                    }
                    else
                    {
                        m_config = new ProviderBaseConfiguration();
                    }
                }


                return m_config;
            }
        }


        public string Name { get; set; }
        public Guid Id { get; set; }
        public string TypeClassName { get; set; }

        private string m_settings;
        public string SerializedSettings { 
            get{
                if (Configuration != null)
                {
                    JavaScriptSerializer oSerializer = new JavaScriptSerializer();
                    return oSerializer.Serialize(m_config);
                }

                return string.Empty;

            }
            set { m_settings = value; }
        }

        public Guid providerId { get; set; }
        public bool IsNew { get; set; }
    }
}
