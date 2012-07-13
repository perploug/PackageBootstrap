using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.UI.SettingTypes {
    public class Checkbox : SettingType {
        
        private System.Web.UI.WebControls.CheckBox cb = new System.Web.UI.WebControls.CheckBox();

        public override string Value {
            get {
                return cb.Checked.ToString();
            }
            set {
                if (value == true.ToString())
                    cb.Checked = true;
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender) {
            cb.ID = setting.GetName().Replace(" ", "");
            return cb;
        }
    }
}
