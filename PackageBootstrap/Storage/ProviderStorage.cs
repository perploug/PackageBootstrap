using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackageBootstrap.Storage;
using PackageBootstrap.ProviderModel;

namespace PackageBootstrap.Storage
{
    public class ProviderStorage<T> : StorageBase where T : IProvider
    {
        public IEnumerable<T> GetProviders(bool loadSettings = true)
        {
            string className = typeof(T).Name.ToString();
                        
            var providers = DataBase.Query<T>("WHERE typeclassName = @0", className);
            List<T> retval = new List<T>();

            foreach (var p in providers)
            {
                p.Initialize(RegisteredProviderTypes.Instance.GetProvider(p.providerId), loadSettings);
                retval.Add(p);
            }

            return retval;
        }

        public void Delete(Guid id)
        {
            ProviderBase<ProviderTypeBase> rp = new ProviderBase<ProviderTypeBase>() { Id = id };
            DataBase.Delete(rp);
        }

        public void Delete(T provider)
        {
            DataBase.Delete(provider);
        }

        public T GetProvider(Guid id, bool loadSettings = true)
        {
            var provider = DataBase.Query<T>("WHERE Id = @0", id).FirstOrDefault();

            if (provider == null)
                return default(T);

            provider.Initialize(RegisteredProviderTypes.Instance.GetProvider(provider.providerId), loadSettings);
            
            return provider;
        }

        public T Save(T provider){

            provider.providerId = provider.Type.Id;
            
            if(provider.IsNew)
                DataBase.Insert(provider);    
            else
                DataBase.Update(provider);

            return provider;
        }
    }
}
