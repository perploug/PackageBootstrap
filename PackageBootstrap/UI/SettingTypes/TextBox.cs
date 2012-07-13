using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.UI.SettingTypes
{
    public class TextField : SettingType
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
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender)
        {
            tb.ID = setting.GetName();
            tb.TextMode = TextBoxMode.SingleLine;
            tb.CssClass = "guiInputText guiInputStandardSize";
            
            if (string.IsNullOrEmpty(tb.Text) && Prevalues.Count > 0)
                tb.Text = Prevalues[0];
            
            return tb;
        }
    }
}
