using CUT_RAIL_MACHINE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using CUT_RAIL_MACHINE.ViewModels;

namespace CUT_RAIL_MACHINE.Services
{
    public class ReadExcelData
    {
        public string basePath;
        public string pathExcel;
        public ReadExcelData()
        {
            basePath = DataConfigModel.PathEmployees;
            pathExcel = Path.Combine(basePath, DataConfigModel.FileNameEmp);
        }
        public UserModel FindProductByID(string idToFind)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(pathExcel, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = false }
                });

                var table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.Length > 1 && row[1].ToString() == idToFind)
                    {
                        return new UserModel
                        {
                            STT = row[0].ToString(),
                            ID = row[1].ToString(),
                            Name = row[2].ToString(),
                            Access = row[6].ToString(),
                        };
                    }
                }
            }
            return null;
        }
    }
}
