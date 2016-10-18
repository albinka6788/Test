using BHIC.Common.Reattempt;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //ReattemptProcess process = new ReattemptProcess();
                //process.start();
                IBackgroundSaveForLaterService process = new BackgroundSaveForLaterService();
                process.TriggerSaveForLaterMail();
                Console.ReadLine();
            }
            catch // (Exception ex)
            {
               
            }
        }
    }
}
