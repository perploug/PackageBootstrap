using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PackageBootstrap.UI.Controls
{
    [System.Web.UI.ValidationProperty("ValidateableProperty")]
    public class ValidateableCheckBox : System.Web.UI.WebControls.CheckBox
    {

        public string ValidateableProperty
        {
            get
            {
                if (this.Checked)
                    return "1";
                else
                    return string.Empty;
            }
        }
    }
}
