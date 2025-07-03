using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using CUT_RAIL_MACHINE.Models;
using System.Runtime.InteropServices;

namespace project.Services
{
    public class SaveDataTestExcel
    {
        string basePath;
        string pathExcelSave;
       // private ExcelPackage package;
        private ExcelWorksheet worksheet;
        public SaveDataTestExcel()
        {
            basePath = DataConfigModel.PathSaveData;
            pathExcelSave = Path.Combine(basePath, DataConfigModel.FileSaveData);
            //package = new ExcelPackage();
            OpenExcel();
        }

        private void OpenExcel()
        {
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            var fileSaveData = new System.IO.FileInfo(pathExcelSave);
            using (var packageSave = new ExcelPackage(fileSaveData))
            {
                if (!fileSaveData.Exists)
                {
                    var worksheetsv = packageSave.Workbook.Worksheets.Add("CUT RAIL MACHINE DATA");
                    worksheetsv.Cells[1, 1].Value = "STT";
                    worksheetsv.Cells[1, 2].Value = "Machine";
                    worksheetsv.Cells[1, 3].Value = "NAME";
                    worksheetsv.Cells[1, 4].Value = "ID";
                    worksheetsv.Cells[1, 5].Value = "ACCESS";
                    worksheetsv.Cells[1, 6].Value = "PO";
                    worksheetsv.Cells[1, 7].Value = "LENGHT";
                    worksheetsv.Cells[1, 8].Value = "TYPE";
                    worksheetsv.Cells[1, 9].Value = "QUANTITY";
                    worksheetsv.Cells[1, 10].Value = "START TIME";
                    worksheetsv.Cells[1, 11].Value = "END TIME";
                }
                packageSave.Save();
            }
        }

        public void SaveData(string machine, string name,  string msnv, string access, string po, string lenght, string type, string quantity,
            string starttime, string endtime /*, string executiontime*/)
        {
            List<UserModel> manufactures = new List<UserModel>
            {
                new UserModel {Machine = machine, Name = name, ID = msnv, Access = access, PO = po,  Lenght = lenght, _Type = type, Quantity = quantity,
                    StartTime = starttime, EndTime = endtime/*, ExecutionTime = executiontime */}
            };
            //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
            var fileInfo2 = new System.IO.FileInfo(pathExcelSave);
            using (var packgeSave = new ExcelPackage(fileInfo2))
            {
                var worksheetSave = packgeSave.Workbook.Worksheets["CUT RAIL MACHINE DATA"];

                int lastRow = worksheetSave.Dimension?.End.Row ?? 0;
                if (lastRow == 1)
                {
                    lastRow = 1;
                }
                int _stt = lastRow;
                foreach (var manfac in manufactures)
                {
                    lastRow++;
                    //

                    worksheetSave.Cells[lastRow, 1].Value = _stt;
                    worksheetSave.Cells[lastRow, 2].Value = manfac.Machine;
                    worksheetSave.Cells[lastRow, 3].Value = manfac.ID;
                    worksheetSave.Cells[lastRow, 4].Value = manfac.Name;
                    worksheetSave.Cells[lastRow, 5].Value = manfac.Access;
                    worksheetSave.Cells[lastRow, 6].Value = manfac.PO;
                    worksheetSave.Cells[lastRow, 7].Value = manfac.Lenght;
                    worksheetSave.Cells[lastRow, 8].Value = manfac._Type;
                    worksheetSave.Cells[lastRow, 9].Value = manfac.Quantity;
                    worksheetSave.Cells[lastRow, 10].Value = manfac.StartTime;
                    worksheetSave.Cells[lastRow, 11].Value = manfac.EndTime;
                    
                    //
                    _stt++;
                }
                packgeSave.Save();
            }
        }
    }
}
