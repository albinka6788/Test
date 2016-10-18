using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.SchedulerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new ScheduledService() 
                };

                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                StartService();
            }
        }

        /// <summary>
        /// Start service if run service as console application
        /// </summary>
        private static void StartService()
        {
            ScheduledService scheduledService = new ScheduledService();
            scheduledService.ServiceManualStart(null);
        }
    }
}
