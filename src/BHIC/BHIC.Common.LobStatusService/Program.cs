using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.LobStatusService
{
    public class Program
    {
        static void Main(string[] args)
        {
            ReadExcel readExcel = new ReadExcel();
            LobDataProvider lobData = new LobDataProvider();

            var result = readExcel.Test();

            ///remove column name from list
            result = result.Skip(1).ToList();

            ///Store data into database
            lobData.WriteDataIntoTable(lobData.PrepareLobStatusData(result));

            Console.ReadKey();
        }
    }
}


