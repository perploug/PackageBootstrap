using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.UI.SettingTypes
{
    public class Password : SettingType
    {
        private TextBox tb = new TextBox();
        
        public override string Value
        {
            get
            {
                return tb.Text;
            }
            set
            {
                tb.Text = value;
                tb.Attributes["value"] = value;
            }
        }


        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender)
        {
            tb.ID = setting.GetName();

            tb.TextMode = TextBoxMode.Password;
            tb.CssClass = "guiInputText guiInputStandardSize";
            return tb;    
        }
    }
}
