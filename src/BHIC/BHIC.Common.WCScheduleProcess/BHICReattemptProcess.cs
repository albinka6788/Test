using BHIC.Common.Reattempt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.WCScheduleProcess
{
    class BHICReattemptProcess : BackGroundProcess
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Override base class method to implement functionality to reattempt the db failures.
        /// </summary>
        /// <returns></returns>
        public override bool Process()
        {
            bool retValue = true;
            this.BackgroundTaskID = BackgroundTaskListItem.ReattemptProcess;
            this.ProcessStartDateTime = DateTime.Now;

            try
            {
                ReattemptProcess process = new ReattemptProcess();
                process.start();
                this.LogProcessStatus(BackgroundTaskStatus.Completed, "Process Completed");
            }
            catch (Exception ex)
            {
                this.LogProcessStatus(BackgroundTaskStatus.Error, ex.ToString());
                retValue = false;
            }

            return retValue;
        }
        #endregion
        #endregion


    }
}
