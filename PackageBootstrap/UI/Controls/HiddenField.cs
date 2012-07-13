using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace PackageBootstrap.UI.Controls
{
    //Custom HiddenField control, since System.Web.UI.WebControls.HiddenField doesn't cast to WebControl

    [ValidationProperty("Value")]
    public class HiddenField : WebControl, INamingContainer
    {

        private System.Web.UI.WebControls.HiddenField hf;

        public HiddenField()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            hf = new System.Web.UI.WebControls.HiddenField();

            this.Controls.Add(hf);
        }

        public string Value
        {
            get
            {
                return hf.Value;
            }
            set
            {
                hf.Value = value;
            }

        }
    }
}
