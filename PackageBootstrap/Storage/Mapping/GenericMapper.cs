// -----------------------------------------------------------------------
// <copyright file="GenericMapper.cs" company="">
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
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GenericMapper : IMapper
    {
        private List<string> triedTypes = new List<string>();
        public PocoTypeMapper GetMapper(Type t)
        {
            if (t == typeof(object) && triedTypes.Count == 0)
                return null;

            var mapper =  GenericMapperCollection.Instance.FindMapper(t);

            if (mapper == null && t.BaseType != null && t.BaseType.IsClass && t.BaseType != t)
                mapper = GetMapper(t.BaseType);
            
            triedTypes.Add(t.Name.Replace("`1", "") + "Mapper");

            if (mapper == null )
                throw new Exception("Generic mapper could not find mapper for type: " + triedTypes[0] + "(searched for: " +  string.Join("\n", triedTypes) + " classes");
            
            triedTypes = new List<string>();

            return mapper;
        }
        
        public void GetTableInfo(Type t, TableInfo ti)
        {
            var mapper = GetMapper(t);

            if (mapper != null)
                mapper.GetTableInfo(t, ti);
        }

        public bool MapPropertyToColumn(System.Reflection.PropertyInfo pi, ref string columnName, ref bool resultColumn)
        {
            //always ignore the IsNew indicator property
            if (pi.Name == "IsNew")
                return false;

            var mapper = GetMapper(pi.DeclaringType);
            if (mapper != null)
                return mapper.MapPropertyToColumn(pi, ref columnName, ref resultColumn);

            return true;
        }

        public Func<object, object> GetFromDbConverter(System.Reflection.PropertyInfo pi, Type SourceType)
        {
            return null;
        }

        public Func<object, object> GetToDbConverter(Type SourceType)
        {
            return null;
        }
    }
}
