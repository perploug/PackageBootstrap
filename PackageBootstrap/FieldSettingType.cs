using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PackageBootstrap.Interfaces;
using PackageBootstrap.ProviderModel;

namespace PackageBootstrap
{
    public abstract class SettingType : ProviderTypeBase, ISettingType
    {
        public virtual string Value { get; set; }
        public abstract System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender);

        private List<string> m_prevalues = new List<string>();
        public List<string> Prevalues
        {
            get { return m_prevalues; }
            set { m_prevalues = value; }
        }
    }
}
