using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PackageBootstrap.UI.Controls {
    [System.Web.UI.ValidationProperty("ValidateableProperty")]
    public class ValidateableCheckBoxList : System.Web.UI.WebControls.CheckBoxList {

        public string ValidateableProperty {
            get {
                return (this.Items.Cast<System.Web.UI.WebControls.ListItem>()
                    .Where(i => i.Selected).Count() > 0) ?
                    "something was selected" : "";
            }
        }
    }
}
