using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.UI.SettingTypes {
    public class Dropdownlist : SettingType {

        private System.Web.UI.WebControls.DropDownList ddl = new System.Web.UI.WebControls.DropDownList();

        private string _val = string.Empty;
        public override string Value {
            get {
                return ddl.SelectedValue;
            }
            set {
               if(!string.IsNullOrEmpty(value))
                   _val = value;
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender) {
            ddl.ID = setting.GetName();

            ddl.Items.Clear();

            foreach(string s in Prevalues){
                ddl.Items.Add(s);
            }

            ddl.SelectedValue = _val;

            return ddl;
        }
    }
}
