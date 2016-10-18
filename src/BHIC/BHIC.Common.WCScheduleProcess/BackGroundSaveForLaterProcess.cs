using BHIC.Common.Reattempt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BHIC.Core.Background;
using BHIC.Contract.Background;
namespace BHIC.Common.WCScheduleProcess
{
    class BackGroundSaveForLaterProcess : BackGroundProcess
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Override base class method to implement functionality to run the save for later for inactive users.
        /// </summary>
        /// <returns></returns>
        public override bool Process()
        {
            bool retValue = true;
            this.BackgroundTaskID = BackgroundTaskListItem.BackGroundSaveForLaterProcess;
            this.ProcessStartDateTime = DateTime.Now;

            try
            {
                //Calling Save For Later Background Process
                IBackgroundSaveForLaterService process = new BackgroundSaveForLaterService();
                process.TriggerSaveForLaterMail();
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
