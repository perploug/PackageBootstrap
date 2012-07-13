using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace PackageBootstrap.UI.SettingTypes {

    public class File : SettingType {

        private Controls.Fileupload fu = new PackageBootstrap.UI.Controls.Fileupload();

        private string _val = "";
        public override string Value {
            get {

                if (fu.UploadControl.HasFile) {
                    string dir = Configuration.Path + "/files/";

                    if (Prevalues.Count > 0)
                        dir += Prevalues[0].Trim('/') + "/";

                    if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(dir)))
                        System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dir));

                    fu.UploadControl.SaveAs(HttpContext.Current.Server.MapPath(dir + fu.UploadControl.FileName));

                    _val = dir + fu.UploadControl.FileName;
                    
                }

                return _val;
            }
            set {
                _val = value;
            }
        }

        public override System.Web.UI.WebControls.WebControl RenderControl(Attributes.Setting setting, Object sender) {

            fu.ID = setting.GetName();

            if (!string.IsNullOrEmpty(_val))
                fu.FileName = _val;
            
            return fu;
        }
    }
}
