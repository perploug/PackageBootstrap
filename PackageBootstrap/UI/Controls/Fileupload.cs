using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;


namespace PackageBootstrap.UI.Controls {
    [ValidationProperty("FileName")]
    public class Fileupload : WebControl, INamingContainer
    {
        private HyperLink link;
        private FileUpload fu;

        public Fileupload()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {  
            fu = new FileUpload();
            fu.CssClass = "fileupload";
            this.Controls.Add(fu);

            link = new HyperLink();
            link.Visible = false;

            if (link.NavigateUrl != string.Empty)
            {
                link.Target = "_blank";
                link.Visible = true;
                link.Text = " Show current file";                
            }

            this.Controls.Add(link);
        }

        public string FileName {
            get {

                EnsureChildControls();

                if (fu.HasFile)
                    return fu.FileName;
                else if (link.NavigateUrl != string.Empty)
                    return link.NavigateUrl;
                else
                    return string.Empty;
            }
            set {

                EnsureChildControls();

                link.Target = "_blank";
                link.NavigateUrl = value;
                link.Visible = true;
                link.Text = " Show current file";

               
            }
        }

        public FileUpload UploadControl {
            get {
                EnsureChildControls();

                return fu;
            }
        }
        
    }
}
