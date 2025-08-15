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
        private readonly string connectionStringOST;
        private readonly string connectionStringERP;
        public RepositoryBase()
        {
            //User ID = ost_pe; Password = ost_pe@spclt
            connectionStringOST = MachineData.connecttionString[0];
            connectionStringERP = MachineData.connecttionString[1];
        }

        protected SqlConnection GetConnectionOST()
        {
            return new SqlConnection(connectionStringOST);
        }

        protected SqlConnection GetConnectionERP()
        {
            return new SqlConnection(connectionStringERP);
        }
    }
}
