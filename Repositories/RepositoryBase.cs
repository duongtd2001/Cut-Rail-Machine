using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CUT_RAIL_MACHINE.Models;
using CUT_RAIL_MACHINE.Services;
using System.IO;

namespace CUT_RAIL_MACHINE.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString;
        public RepositoryBase()
        {
            //User ID = ost_pe; Password = ost_pe@spclt
            connectionString = $"{DataConfigModel.DataSource};{DataConfigModel.InitialCatalog};{DataConfigModel.PersistSecurityInfo};" +
                $"{DataConfigModel.UserID};{DataConfigModel.Password};";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        } 
    }
}
