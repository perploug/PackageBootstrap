using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackageBootstrap.UI.Controls;

namespace PackageBootstrap.UI.SettingTypes.Pickers
{
    public class ContentWithXpath : SettingType
    {
        private ContentPickerWithXpathOption cc = new ContentPickerWithXpathOption();

        private string _val = string.Empty;
        public override string Value
        {
            get
            {
                return cc.Value;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _val = value;
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender)
        {
            cc.ShowXPath = true;
            cc.ID = setting.GetName().Replace(" ", "_");

            cc.Value = _val;
            return cc;
        }
    }
}
