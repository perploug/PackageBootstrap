using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PackageBootstrap.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class Setting : System.Attribute
    {
        string name;
        public string description;
        public string control;
        public string assembly;
        public string prevalues;

        public Setting(string name)
        {
            this.name = name;
            description = "";
            control = "PackageBootstrap.UI.SettingTypes.TextField";
            prevalues = "";
        }

        public string GetName()
        {
            return name;
        }

        public List<string> GetPrevalues() {
            List<string> list = new List<string>();
            list.AddRange( prevalues.Split(',') );

            return list;
        }
        
        public SettingType GetSettingType()
        {           
            Assembly a = string.IsNullOrEmpty(assembly) ? Assembly.GetExecutingAssembly() : Assembly.Load(assembly);
            SettingType fst = (SettingType)a.CreateInstance(control);

            if (fst != null)
                fst.Prevalues = GetPrevalues();

            return fst;
        }

    }
}
