using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.ProviderModel
{
    public class RegisteredProviderTypes : ProviderCollection<ProviderTypeBase>
    {
        #region Constructors (1)

        public RegisteredProviderTypes() : base() { }

        #endregion Constructors

        #region Properties (1)

        public static RegisteredProviderTypes Instance
        {
            get { return Singleton<RegisteredProviderTypes>.Instance; }
        }

        #endregion Properties
    }
}
