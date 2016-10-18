using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using BHIC.Common.DataAccess;

namespace BHIC.EncryptDecrypt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
            //    BHICDBBase dbConnection = new BHICDBBase();

            //    dbConnection.DBName = "GuinnessDB";

            //    dbConnection.OpenConnection();

            //    MessageBox.Show("DB Connection Successful");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EncryptionDecryptionForm());
        }
    }
}
