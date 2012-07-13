// -----------------------------------------------------------------------
// <copyright file="StorageBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PackageBootstrap.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PackageBootstrap.Storage.Mapping;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StorageBase : IDisposable
    {
        public virtual string ConnectionString
        {
            get
            {
                //default umbraco5 hive connection
                return "nhibernate.ConnString";
            }
        }
       
        private PetaPoco.Database m_database = null;
        public PetaPoco.Database DataBase
        {
            get
            {
                if (!Connected)
                    Connect();

                if (m_database == null || !Connected)
                    throw new Exception("Petapoco has no valid connection string");

                return m_database;
            }
        }

        public bool Connected = false;
        public void Connect()
        {
            var dbdns = ConnectionString;
            if (dbdns.Contains('='))
                m_database = new PetaPoco.Database(dbdns, "System.Data.SqlClient");
            else
                m_database = new PetaPoco.Database(dbdns);

            PetaPoco.Database.Mapper = new GenericMapper();
            Connected = true;
        }
        

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
