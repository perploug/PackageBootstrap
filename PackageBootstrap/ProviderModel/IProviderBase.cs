using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageBootstrap.ProviderModel
{
    public interface IProvider
    {
        void Initialize(object ProviderType, bool loadSettings = true);
        string Name { get; set; }
        Guid Id { get; set; }
        string TypeClassName { get; set; }
        string SerializedSettings { get; set; }

        ProviderTypeBase Type { get;}
        Guid providerId { get; set; }
        bool IsNew { get; set; }
    }
}
