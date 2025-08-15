using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class ExcelRW
    {

        private Dictionary<string, string> Employees = new Dictionary<string, string>();
        private Dictionary<string, string> MasterProduct = new Dictionary<string, string>();

        public List<double> datas = new List<double>();

        private MachineData machineData;
        private string PathCombineEmployees;
        private string PathMasterType;

        public ExcelRW(MachineData _machineData)
        {
            machineData = _machineData;
            PathCombineEmployees = Path.Combine(machineData.EMPLOYEES_PATH, machineData.EMPLOYEES_FILENAME);
            PathMasterType = Path.Combine(machineData.MASTER_TYPE_PATH, machineData.MASTER_TYPE_FILENAME);
        }

        public Dictionary<string, string> FindEmployeesByID(string idToFind)
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(PathCombineEmployees, FileMode.Open, FileAccess.Read))
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
                        Employees["STT"] = row[0].ToString();
                        Employees["ID"] = row[1].ToString();
                        Employees["Name"] = row[2].ToString();
                        Employees["Access"] = row[6].ToString();
                    }
                }
                return Employees;
            }
        }

        public Dictionary<string, string> FindProductByName(string NameToFind)
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(PathMasterType, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = false }
                });

                var table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.Length > 1 && row[1].ToString() == NameToFind)
                    {
                        MasterProduct["PartID"] = row[0].ToString();
                        MasterProduct["Name"] = row[1].ToString();
                        MasterProduct["Lenght"] = row[2].ToString();
                        MasterProduct["Type"] = row[3].ToString();
                    }
                }
                return MasterProduct;
            }
        }
    }
}
