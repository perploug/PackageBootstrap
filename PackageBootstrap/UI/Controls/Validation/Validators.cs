using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.UI.Controls.Validation
{

        public class RequiredValidator : RequiredFieldValidator
        {
         
            protected override void OnPreRender(EventArgs e)
            {
                base.OnPreRender(e);

                var ctl = NamingContainer.FindControl(ControlToValidate) as WebControl;
                var lblctl = NamingContainer.FindControl(LabelControlToValidate) as WebControl;

                if (ctl != null)
                {
                    if (IsValid)
                    {

                        ctl.CssClass = RemoveClass(ctl.CssClass, "contourError");
           
                        if (lblctl != null)
                            lblctl.CssClass = RemoveClass(lblctl.CssClass, "contourError");

                        if (Page.IsPostBack)
                        {
                            ctl.CssClass = AddClass(ctl.CssClass, "contourValid");

                            if (lblctl != null)
                                lblctl.CssClass = AddClass(lblctl.CssClass, "contourValid");
                        }

                    }
                    else
                    {
                        ctl.CssClass = RemoveClass(ctl.CssClass, "contourValid");
                        if (lblctl != null)
                            lblctl.CssClass = RemoveClass(lblctl.CssClass, "contourValid");

                        ctl.CssClass = AddClass(ctl.CssClass, "contourError");
                        if(lblctl != null)
                            lblctl.CssClass = AddClass(lblctl.CssClass, "contourError");

                    }
                }
            }

            public string LabelControlToValidate { get; set; }

            private string AddClass(string CssClass, string ClassToAdd)
            {
                if (CssClass.Split(' ').FirstOrDefault(c => c.Equals(ClassToAdd)) == null)
                {
                    CssClass = String.IsNullOrEmpty(CssClass) ? ClassToAdd : CssClass + " " + ClassToAdd;
                } 

                return CssClass;
            }

            private string RemoveClass(string CssClass, string ClassToRemove)
            {
                var className = String.Join(" ", CssClass.Split(' ')
                            .Where(c => !c.Equals(ClassToRemove)).ToArray());
                return className;
            }
        }

        public class RegexValidator : RegularExpressionValidator
        {
            protected override void OnPreRender(EventArgs e)
            {
                base.OnPreRender(e);
                var ctl = NamingContainer.FindControl(ControlToValidate) as WebControl;
                var lblctl = NamingContainer.FindControl(LabelControlToValidate) as WebControl;
                               
                if (ctl != null)
                {
                    this.Validate();

                    if (IsValid)
                    {
                        ctl.CssClass = RemoveClass(ctl.CssClass, "contourError");

                        if (lblctl != null)
                            lblctl.CssClass = RemoveClass(lblctl.CssClass, "contourError");

                        if (Page.IsPostBack)
                        {
                            ctl.CssClass = AddClass(ctl.CssClass, "contourValid");

                            if (lblctl != null)
                                lblctl.CssClass = AddClass(lblctl.CssClass, "contourValid");
                        }

                    }
                    else
                    {
                        ctl.CssClass = RemoveClass(ctl.CssClass, "contourValid");
                        if (lblctl != null)
                            lblctl.CssClass = RemoveClass(lblctl.CssClass, "contourValid");

                        ctl.CssClass = AddClass(ctl.CssClass, "contourError");
                        if (lblctl != null)
                            lblctl.CssClass = AddClass(lblctl.CssClass, "contourError");

                    }
                }
            }

            public string LabelControlToValidate { get; set; }

            private string AddClass(string CssClass, string ClassToAdd)
            {
                if (CssClass.Split(' ').FirstOrDefault(c => c.Equals(ClassToAdd)) == null)
                {
                    CssClass = String.IsNullOrEmpty(CssClass) ? ClassToAdd : CssClass + " " + ClassToAdd;
                }

                return CssClass;
            }

            private string RemoveClass(string CssClass, string ClassToRemove)
            {
                var className = String.Join(" ", CssClass.Split(' ')
                            .Where(c => !c.Equals(ClassToRemove)).ToArray());
                return className;
            }
        }
    
}
