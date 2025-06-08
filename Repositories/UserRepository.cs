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

namespace CUT_RAIL_MACHINE.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private ReadExcelData readExcelData;
        //private UserModel _user;
        //
        public void Add(UserModel userModel)
        {
            try
            {
                string query = @"INSERT INTO dbo.ASS_LX (Machine, ID, Name, ProductName, Time) 
                         VALUES (@machine, @id, @name, @productname, @time)";

                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@machine", userModel.Machine);
                    cmd.Parameters.AddWithValue("@id", userModel.ID);
                    cmd.Parameters.AddWithValue("@Name", userModel.Name);
                    //cmd.Parameters.AddWithValue("@name", userModel.PO);
                    cmd.Parameters.AddWithValue("@productname", userModel.ProducName);
                    //cmd.Parameters.AddWithValue("@Time_check", userModel.StartTime);
                    cmd.Parameters.AddWithValue("@time", userModel.Time);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                //MessageBox.Show("Server not connected!!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            readExcelData = new ReadExcelData();
            //Excel
            bool validUser = false;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using (var stream = File.Open(readExcelData.pathExcel, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    var table = dataSet.Tables[0];

                    foreach (DataRow row in table.Rows)
                    {
                        if (row[1].ToString() == credential.UserName)
                        {
                            validUser = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return validUser;
        }
        
        public void Edit(UserModel userModel)
        {
            
            string query = @"UPDATE dbo.ASS_LX 
                         SET Machine = @machine, Name = @name, LotNo = @lotno, Time = @time 
                         WHERE ID = @ID";
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@machine", userModel.Machine);
                    cmd.Parameters.AddWithValue("@name", userModel.Name);
                    //cmd.Parameters.AddWithValue("@PO", userModel.ProducName);
                    cmd.Parameters.AddWithValue("@lotno", userModel.LotNo);
                    cmd.Parameters.AddWithValue("@time", userModel.Time);
                    cmd.Parameters.AddWithValue("@id", userModel.ID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            //UserModel user = null;
            ////Excel
            //readExcelData = new ReadExcelData();
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            //using (var stream = File.Open(readExcelData.pathExcel, FileMode.Open, FileAccess.Read))
            //using (var reader = ExcelReaderFactory.CreateReader(stream))
            //{
            //    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
            //    {
            //        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
            //    });

            //    var table = dataSet.Tables[0];

            //    foreach (DataRow row in table.Rows)
            //    {
            //        if (row[2].ToString() == username)
            //        {
            //            user = new UserModel
            //            {
            //                Name = row[2].ToString()
            //            };
            //            break;
            //        }
            //    }
            //}
            //return user;
            throw new NotImplementedException();
        }

        public void Remove(string Id)
        {
            string query = @"DELETE FROM dbo.ASS_LX 
                         WHERE ID = @id AND Time = (
                             SELECT MAX(Time) FROM dbo.ASS_LX WHERE ID = @id)";
            try
            {
                using (SqlConnection conn = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
            
        }

        public bool StatusConnectSQL()
        {
            bool validConn = false;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    if(conn.State == System.Data.ConnectionState.Open)
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
