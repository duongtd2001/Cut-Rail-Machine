using ExcelDataReader;
using CUT_RAIL_MACHINE.Services;
using CUT_RAIL_MACHINE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace CUT_RAIL_MACHINE.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public string Error = null;
        public void Add()
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            throw new NotImplementedException();
        }

        public void Edit()
        {
            throw new NotImplementedException();
        }

        public string GetProductNameByPO(string Po)
        {
            string ProductName = null;
            try
            {
                string sqlERP = @"
                        SELECT h.[PHTX] 
                        FROM [MANUFA].[MANUFA_61].[DT_REQ_HED] h 
                        LEFT JOIN [MANUFA].[MANUFA_61].[DT_ORDER_HED] oh 
                            ON oh.[VBELN] = h.[KDAUF] 
                        LEFT JOIN [MANUFA].[MANUFA_61].[GRB_PRODUCT] pr 
                            ON pr.[MATNR] = h.[PHCD]
                        LEFT JOIN [MANUFA].[MANUFA_61].[DT_ORDER_DTL] ol 
                            ON ol.[VBELN] = h.[KDAUF]
                        WHERE h.[AUFNR] = @PONumber";


                using (SqlConnection conn = GetConnectionERP())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlERP, conn))
                    {
                        cmd.Parameters.AddWithValue("PONumber", Po);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                ProductName = dr["PHTX"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = $"* ERP Server connection error.";
            }
            return ProductName;
        }

        public void GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(string Id)
        {
            throw new NotImplementedException();
        }

        public bool StatusConnectERP()
        {
            bool validConn = false;
            using (SqlConnection conn = GetConnectionERP())
            {
                try
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        validConn = true;
                    }
                    else
                    {
                        validConn = false;
                    }
                }
                catch
                {
                }
            }
            return validConn;
        }

        public bool StatusConnectOST()
        {
            bool validConn = false;
            using (SqlConnection conn = GetConnectionOST())
            {
                try
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        validConn = true;
                    }
                    else
                    {
                        validConn = false;
                    }
                }
                catch
                {
                }
            }
            return validConn;
        }
    }
}
