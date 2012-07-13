// -----------------------------------------------------------------------
// <copyright file="PocoTypeMapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Storage.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PetaPoco;
    using PackageBootstrap.ProviderModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class PocoTypeMapper : ProviderTypeBase
    {
        public PocoTypeMapper()
        {
            Name = "Pocotype mapper";
            Id = Guid.NewGuid();
            Description = "Generic mapper for petapoco";
        }

        public abstract void GetTableInfo(Type t, TableInfo ti);
        public abstract bool MapPropertyToColumn(System.Reflection.PropertyInfo pi, ref string columnName, ref bool resultColumn);
    }
}
