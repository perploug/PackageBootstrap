using System;
using System.Collections.Generic;
using System.Text;
using AjaxControlToolkit;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace PackageBootstrap.UI.Controls
{
    [ValidationProperty("SelectedDate")]
    public class DatePicker: System.Web.UI.WebControls.WebControl, System.Web.UI.INamingContainer 
    {
        private System.Web.UI.WebControls.TextBox tb;
        private CalendarExtender ca;
        private System.Web.UI.WebControls.Calendar cal;
        private System.Web.UI.WebControls.HiddenField hf;

        public DatePicker()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {   
            tb = new System.Web.UI.WebControls.TextBox();
            tb.ID = "tb" + this.ID;
            tb.CssClass = "text";
            //this.Controls.Add(tb);

            ca = new CalendarExtender();
            DateTimeFormatInfo di = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
            ca.Format = di.ShortDatePattern;
            ca.TargetControlID = tb.ID;
            ca.ID = "ce" + this.ID;
            //this.Controls.Add(ca);


            hf = new System.Web.UI.WebControls.HiddenField();
            hf.ID = "hidden" + this.ID;

            cal = new System.Web.UI.WebControls.Calendar();
            cal.ID = "cal" + this.ID;
            cal.CssClass = "calendar";
            cal.SelectionChanged += new EventHandler(cal_SelectionChanged);
            //this.Controls.Add(cal);
        }

        void cal_SelectionChanged(object sender, EventArgs e)
        {
            if (cal.SelectedDates.Count > 0)
                hf.Value = cal.SelectedDate.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Page.Header == null)
            {
                this.Controls.Add(cal);
                this.Controls.Add(hf);
            }
            else
            {
                this.Controls.Add(tb);
                this.Controls.Add(ca);
            }
        }

        public DateTime? SelectedDate {
            get
            {
                EnsureChildControls();
                DateTime selected;

                if (cal.SelectedDates.Count > 0)
                {   
                    selected = cal.SelectedDate;
                    return selected;
                }

                if (DateTime.TryParse(tb.Text, out selected))
                {
                    return selected;
                }

                return null;
            }
            set
            {
                EnsureChildControls();
                if (value != null)
                {

                    DateTimeFormatInfo di = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
                    tb.Text = ((DateTime)value).ToString(di.ShortDatePattern);
                    
                    ca.SelectedDate = value;

                    cal.SelectedDate = (DateTime)value;
                }
            }
        }
    }
}
