using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.Interfaces
{
    public interface ISettingType
    {
        string Value { get; set; }
        WebControl RenderControl(Attributes.Setting setting, Object sender);
    }
}