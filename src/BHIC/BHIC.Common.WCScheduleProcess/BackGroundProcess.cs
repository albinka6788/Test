using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.WCScheduleProcess
{
    #region ENums

    /// <summary>
    /// Enum to define different types of background processes.
    /// </summary>
    public enum BackgroundTaskListItem
    {
        WCEmailNotification,
        ReattemptProcess,
        BackGroundSaveForLaterProcess,
        ReloadCache,
    }

    /// <summary>
    /// Enum to define different status of the background process.
    /// </summary>
    public enum BackgroundTaskStatus
    {
        Started,
        Completed,
        Error
    }

    #endregion

    /// <summary>
    /// Abstract class for defining common functionalities of all background processes.
    /// </summary>
    internal abstract class BackGroundProcess
    {
        #region Variables

        private BackgroundTaskListItem backgroundTaskID;
        private DateTime processStartDateTime;
        private DateTime processEndDateTime;
        private int numberOfRecordsProcessed;

        #endregion

        #region Properties

        /// <summary>
        /// Batch Process ID From List Item
        /// </summary>
        public virtual BackgroundTaskListItem BackgroundTaskID
        {
            get
            {
                return backgroundTaskID;
            }
            set
            {
                backgroundTaskID = value;
            }
        }

        /// <summary>
        /// Process Start Date Time
        /// </summary>
        public virtual DateTime ProcessStartDateTime
        {
            get
            {
                return processStartDateTime;
            }
            set
            {
                processStartDateTime = value;
            }
        }

        /// <summary>
        /// Process Completion or End Date Time
        /// </summary>
        public virtual DateTime ProcessEndDateTime
        {
            get
            {
                return processEndDateTime;
            }
            set
            {
                processEndDateTime = value;
            }
        }

        /// <summary>
        /// Number of Record Processed throug the Batch Process
        /// </summary>
        public virtual int NumberOfRecordsProcessed
        {
            get
            {
                return numberOfRecordsProcessed;
            }
            set
            {
                numberOfRecordsProcessed = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Abstract method for running the requested process.
        /// </summary>
        /// <returns></returns>
        public abstract bool Process();

        #endregion

        #region Internal Methods

        /// <summary>
        /// Log current process status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        internal void LogProcessStatus(BackgroundTaskStatus status, string message)
        {
            this.ProcessEndDateTime = DateTime.Now;

            TimeSpan ts = this.processEndDateTime - this.processStartDateTime;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(status.ToString() + " : ");
            sb.AppendLine();
            sb.AppendLine(string.Format("Process {0} Started at {1}", this.GetType().Name, this.processStartDateTime.ToString()));

            switch (status)
            {
                case BackgroundTaskStatus.Error:
                    sb.AppendLine(string.Format("And Error occurred while running the Process at {0}", this.processEndDateTime.ToString()));
                    sb.AppendLine();
                    sb.AppendLine("Error Message : ");
                    sb.AppendLine("\t" + message);
                    BHIC.Common.Logging.LoggingService.Instance.Fatal(string.Format("{0}", sb.ToString()));
                    break;
                case BackgroundTaskStatus.Started:
                case BackgroundTaskStatus.Completed:
                default:
                    sb.AppendLine(string.Format("And Completed successfully at {0} with message {1}", this.processEndDateTime.ToString(), message));
                    sb.AppendLine("Time taken by Process " + ts.Days + " Days, " + ts.Hours + " Hours, " + ts.Minutes + " Minutes, " +
                                                                    ts.Seconds + " Seconds, " + ts.Milliseconds + " Milliseconds");
                    BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("{0}", sb.ToString()));
                    break;
            }

        }

        #endregion

        #endregion
    }
}
