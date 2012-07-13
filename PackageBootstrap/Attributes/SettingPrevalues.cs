using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class SettingPrevalues : System.Attribute
    {
        string name;
        public SettingPrevalues(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }


        /*
        public SettingType GetSettingType()
        {
            Assembly a = string.IsNullOrEmpty(assembly) ? Assembly.GetExecutingAssembly() : Assembly.Load(assembly);
            SettingType fst = (SettingType)a.CreateInstance(view);

            if (fst != null)
                fst.Prevalues = GetPrevalues();

            return fst;
        }*/

    }
}
