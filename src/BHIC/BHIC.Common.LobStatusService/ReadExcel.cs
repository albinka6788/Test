using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.LobStatusService
{
    public class ReadExcel
    {
        public List<LobStatus> Test()
        {
            string filePath = @"C:\Users\anuj.singh\Desktop\LobStatus\ls.xlsx";
            List<LobStatus> lobStatusLst = new List<LobStatus>();

            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            //1. Reading from a binary Excel file ('97-2003 format; *.xls)
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet();
            excelReader.IsFirstRowAsColumnNames = true;

            //5. Data Reader methods
            while (excelReader.Read())
            {
                lobStatusLst.Add(
                    new LobStatus
                    {
                        StateName = excelReader.GetString(0).ToString(),
                        Abbreviation = excelReader.GetString(1).ToString(),
                        BOP = excelReader.GetString(2).ToString(),
                        WC = excelReader.GetString(3).ToString(),
                        CA = excelReader.GetString(4).ToString(),
                    });
            }

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            return lobStatusLst;
        }
    }
}
