using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AjaxControlToolkit;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.WebResource("PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.js", "text/javascript")]


namespace PackageBootstrap.UI.Controls.Validation
{
    [Designer(typeof(ValidationFieldExtenderDesigner))]
    [ClientScriptResource("PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior", "PackageBootstrap.UI.Controls.Validation.ValidationFieldExtenderBehavior.js")]
    [TargetControlType(typeof(IValidator))]
    public class ValidationFieldExtender : ExtenderControlBase
    {
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("TargetLabelID")]
        [ElementReference]
        [RequiredProperty]
        [IDReferenceProperty(typeof(Label))]
        public string TargetLabelID
        {
            get { return GetPropertyValue("TargetLabelID", string.Empty); }
            set { SetPropertyValue("TargetLabelID", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("InvalidTextBoxCssClass")]
        [RequiredProperty]
        public string InvalidTextBoxCssClass
        {
            get { return GetPropertyValue("InvalidTextBoxCssClass", string.Empty); }
            set { SetPropertyValue("InvalidTextBoxCssClass", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("ValidTextBoxCssClass")]
        [RequiredProperty]
        public string ValidTextBoxCssClass
        {
            get { return GetPropertyValue("ValidTextBoxCssClass", string.Empty); }
            set { SetPropertyValue("ValidTextBoxCssClass", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("InvalidLabelCssClass")]
        [RequiredProperty]
        public string InvalidLabelCssClass
        {
            get { return GetPropertyValue("InvalidLabelCssClass", string.Empty); }
            set { SetPropertyValue("InvalidLabelCssClass", value); }
        }

        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("ValidLabelCssClass")]
        [RequiredProperty]
        public string ValidLabelCssClass
        {
            get { return GetPropertyValue("ValidLabelCssClass", string.Empty); }
            set { SetPropertyValue("ValidLabelCssClass", value); }
        }

    }
}
