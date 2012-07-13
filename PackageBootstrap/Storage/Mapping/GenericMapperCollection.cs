// -----------------------------------------------------------------------
// <copyright file="GenericMapperCollection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Storage.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PackageBootstrap.ProviderModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GenericMapperCollection : ProviderCollection<PocoTypeMapper>
    {
        #region Constructors (1)

        public GenericMapperCollection() : base() { }

        #endregion Constructors

        #region Properties (1)

        public static GenericMapperCollection Instance
        {
            get { return Singleton<GenericMapperCollection>.Instance; }
        }

        public PocoTypeMapper FindMapper(Type t){
            var mapperName = t.Name.Replace("`1", "") + "Mapper";
            return Instance.GetProviders().Where(x => x.GetType().Name == mapperName).FirstOrDefault();
        }

        #endregion Properties
    }
}
