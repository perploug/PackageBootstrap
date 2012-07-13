﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace PackageBootstrap.UI.Controls
{

    public class ContentPickerWithXpathOption : WebControl
    {

        private bool _showXPath = true;
        
        public bool ShowXPath { 
            get{return _showXPath;} 
            set{_showXPath = value;} 
        }

        private umbraco.controls.ContentPicker cc;
        private TextBox tb;

        public ContentPickerWithXpathOption()
        {
          
            EnsureChildControls();
        }

        protected override void OnInit(EventArgs e)
        {
           

           Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"contentpickerxpath", @"
                    
                function showXpathTextbox(control)
                {
                        $('.picker',$('#' + control)).hide();
                        $('.xpath',$('#' + control)).show();
                }

                function showContentPicker(control)
                {
                        $('.xpath',$('#' + control)).hide();
                        $('.xpath input',$('#' + control)).val('');
                        $('.picker',$('#' + control)).show();
                }
            ", true);

            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            cc = new umbraco.controls.ContentPicker();
            this.Controls.Add(cc);


            if(_showXPath){
            tb = new TextBox();
            tb.CssClass = "guiInputText guiInputStandardSize";
            this.Controls.Add(tb);
            }
        }

        protected override void RenderChildren(System.Web.UI.HtmlTextWriter writer)
        {
           
            writer.Write(string.Format("<div class=\"picker\"{0}>",  tb.Text != string.Empty ? " style=\"display:none\"" : "" ));
            cc.RenderControl(writer);

            if(_showXPath){
                writer.Write(string.Format(
                    "&nbsp<a href=\"javascript:showXpathTextbox('{0}');\">Or enter Xpath</a>",
                    this.ClientID));

                writer.Write("</div>");

                writer.Write(string.Format("<div class=\"xpath\"{0}>", !(tb.Text != string.Empty) ? " style=\"display:none\"" : ""));
                tb.RenderControl(writer);

                writer.Write(string.Format(
                    "&nbsp;<a href=\"javascript:showContentPicker('{0}');\">Or pick node</a>",
                    this.ClientID));
            }

            writer.Write("</div>");
        }

     


       private string _val = string.Empty;
       public string Value {
            get {

                if(_showXPath && tb.Text != string.Empty)
                    return tb.Text;
                else
                    return cc.Text;
            }
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    _val = value;

                    int nodeId;

                    if (int.TryParse(_val, out nodeId))
                        cc.Text = _val;
                    else if(ShowXPath)
                        tb.Text = _val;
                }
            }
        }

    }
}
