#region Using directives

using BHIC.Common.Mailing;
using System;
using System.IO;

#endregion

namespace BHIC.Common.WCScheduleProcess
{
    internal class BHICEmailNotification : BackGroundProcess
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Override base class method to implement functionality to sent Email notification.
        /// </summary>
        /// <returns></returns>
        public override bool Process()
        {
            bool retValue = true;
            this.BackgroundTaskID = BackgroundTaskListItem.WCEmailNotification;
            this.ProcessStartDateTime = DateTime.Now;

            try
            {
                //To Do : Implement logic to send log notification through Email. 
                //EmailNotificationProcess emailnotificationprocess = new EmailNotificationProcess();
                //emailnotificationprocess.SendEmailNotification();

                //To Do: Implement log process here
                // this.LogProcessStatus(BackgroundTaskStatus.Completed, "WC Email Notification succesfully processed.");
            }
            catch (Exception)
            {
                // this.LogProcessStatus(BackgroundTaskStatus.Error, ex.Message.ToString());
                retValue = false;
            }

            return retValue;
        }

        #endregion

        #endregion
    }
}
