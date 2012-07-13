using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.UI.SettingTypes
{
    public class TextArea : SettingType
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
            tb.TextMode = TextBoxMode.MultiLine;
            tb.CssClass = "guiInputText guiInputStandardSize";
            tb.Rows = 7;
            
            if (string.IsNullOrEmpty(tb.Text) && this.Prevalues.Count > 0)
                tb.Text = this.Prevalues[0];
            
            return tb;
        }
    }
}
