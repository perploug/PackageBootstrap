using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using umbraco.BusinessLogic.Utils;

using System.Collections;
using PackageBootstrap.Exceptions;


namespace PackageBootstrap.ProviderModel
{
    public abstract class ProviderCollection<T> where T : ProviderTypeBase
    {
		#region Fields (1) 

        private readonly Dictionary<Guid, T> m_providers = new Dictionary<Guid, T>();

		#endregion Fields 

		#region Constructors (1) 

        public ProviderCollection()
        {
            initialize();
        }

		#endregion Constructors 

		#region Methods (7) 

		// Public Methods (5) 

        public bool ContainsProvider(Guid id)
        {
            return (m_providers.ContainsKey(id));
        }

        public bool ContainsProvider(string type)
        {
            if (type.Contains('.'))
               return (m_providers.Values.Where(x => x.GetType().ToString() == type).FirstOrDefault() != null);
            else
                return (m_providers.Values.Where(x => x.GetType().ToString().EndsWith("." + type)).FirstOrDefault() != null);
        }

        public T GetProvider(Guid id) {

            if (m_providers.ContainsKey(id))
            {
                return m_providers[id];
            }
            else
            {
                throw new ProviderException(
                    String.Format("No provider<"+ typeof(T) +"> with id '{0}' found", id.ToString()));
            }
        }

        public T GetProvider(string type) {
            T provider = null;

            if(type.Contains('.'))
                provider = m_providers.Values.Where(x => x.GetType().ToString() == type).FirstOrDefault();
            else
                provider = m_providers.Values.Where(x => x.GetType().ToString().EndsWith("." + type)).FirstOrDefault(); 

          if (provider != null) {
            return provider;
          } else {
            throw new ProviderException(
                String.Format("No provider with type '{0}' found", type.ToString()));
          }
        }

        public List<T> GetProviders()
        {
            List<T> retVal = new List<T>();

            IDictionaryEnumerator ide = m_providers.GetEnumerator();
            while (ide.MoveNext())
            {
                retVal.Add((T) ide.Value);
            }

            retVal.Sort(delegate(T x, T y) { return ((ProviderTypeBase)x).Name.CompareTo(((ProviderTypeBase)y).Name); });

            return retVal;
        }

        public List<T> GetProvidersByIndex()
        {
            List<T> retVal = GetProviders();
            retVal.Sort(delegate(T x, T y) { return ((ProviderTypeBase)x).Index.CompareTo(((ProviderTypeBase)y).Index); });
            return retVal;
        }
		// Private Methods (2) 

        private void initialize()
        {
            // Get all datatypes from interface
            var types = TypeFinder.FindClassesOfType<T>(false)
                .ToArray();
            registerProviders(types);
        }

        private void registerProviders(IEnumerable<Type> types)
        {
                foreach (Type t in types)
                {
                    T typeInstance = null;
                    try
                    {
                        if (t.IsVisible && !t.IsAbstract && !t.ContainsGenericParameters)
                        {
                            typeInstance = Activator.CreateInstance(t) as T;
                        }
                    }
                    catch (Exception ee)
                    {
                        Log.Error("Can't initialize provider '" + t.FullName + "': " + ee.ToString());
                    }
                    if (typeInstance != null)
                    {
                        try
                        {
                            /*

                            if (typeof(T) != typeof(ItemDataResolverProvider) && Licensing.InfralutionLicensing.IsLight())
                            {
                                var assName = typeInstance.GetType().Assembly.GetName().Name;
                                if (!Licensing.InfralutionLicensing.IsCoreCourierAssembly(assName))
                                    throw new Umbraco.Licensing.Exceptions.InvalidLicenseException(string.Format(Licensing.InfralutionLicensing.LICENSE_APIERROR, assName));
                            }
                            */

                            if (typeInstance.GetType().BaseType != typeof(T))
                            {
                                if (m_providers.ContainsKey(typeInstance.Id))
                                    m_providers.Remove(typeInstance.Id);
                            }

                            m_providers.Add(typeInstance.Id, typeInstance);

                        }
                        catch (Exception ee)
                        {
                            Log.Error("Can't import provider '" + t.FullName + "' id: " + typeInstance.Id.ToString() + ": " + ee.ToString());
                        }
                    }
                }
            
        }

		#endregion Methods 
    }
}
