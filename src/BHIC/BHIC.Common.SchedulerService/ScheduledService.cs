#region Using directives

using BHIC.Common.WCScheduleProcess;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using timers = System.Timers;

using System.Windows.Forms;

#endregion

namespace BHIC.Common.SchedulerService
{
    public partial class ScheduledService : ServiceBase
    {
        #region Variables

        timers.Timer scheduler = new timers.Timer();
        WcServiceConfigLoader wcServiceConfigLoader;

        #endregion

        #region Constructors

        public ScheduledService()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Manually run service
        /// </summary>
        /// <param name="args"></param>
        public void ServiceManualStart(string[] args)
        {
            OnStart(args);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Performs set of task, when schedular start.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            WriteEventLog("Service started at {0}");
            // System.Diagnostics.Debugger.Launch();
            // Get the application configuration file.
            System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None) as Configuration;

            // Read and display the custom section.
            wcServiceConfigLoader =
               ConfigurationManager.GetSection(WcServiceConfigLoader.SectionName) as WcServiceConfigLoader;

            int timerInterval = 1;
            int.TryParse((ConfigurationManager.AppSettings.AllKeys.Contains("TimerInterval") ?
                                ConfigurationManager.AppSettings["TimerInterval"].ToString() : "60"), out timerInterval);
            scheduler.Elapsed += new timers.ElapsedEventHandler(SchedularCallback);
            scheduler.Interval = (1000 * timerInterval);
            scheduler.Enabled = true;
            scheduler.Start();

            // In case service run as console application then hold service execution till user press enter key.
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Press enter to stop service");
                Console.Read();
            }
        }

        /// <summary>
        /// Performs set of task, when schedular stop.
        /// </summary>
        protected override void OnStop()
        {
            WriteEventLog("WC Window Service is now Stopped by User", "Service");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Perform tasks periodically on given interval
        /// </summary>
        /// <param name="e"></param>
        private void SchedularCallback(object sender, EventArgs e)
        {
           // MessageBox.Show("Process start");

            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime;

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["RecordProcessStatus"]))
            {
                WriteEventLog(string.Format("WC Window Service Timer event fired at {0}", startDateTime), "TimerEvent");
            }

            foreach (ConnectionManagerEndpointElement endpointElement in wcServiceConfigLoader.ConnectionManagerEndpoints)
            {
                if(endpointElement.IsSpecificTime && isScheduledTime(endpointElement))
                {
                    LaunchWCService(endpointElement);
                }
                else if (endpointElement.IsServiceNeedToRun && endpointElement.LastRunStartDateTime <= endpointElement.LastRunEndDateTime &&
                (endpointElement.LastRunEndDateTime.AddMinutes(endpointElement.RepeatTaskInterval) <= DateTime.Now) &&
                (endpointElement.IsMultipleRun || (!endpointElement.IsMultipleRun && !endpointElement.IsServiceRun)))
                {
                    LaunchWCService(endpointElement);
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["RecordProcessStatus"]))
            {
                endDateTime = DateTime.Now;
                TimeSpan ts = endDateTime - startDateTime;

                WriteEventLog(
                    string.Format("WC Window Service Timer event completed at {0} and taken {1} Days {2} hours {3} minutes {4} seconds and {5} milliseconds for execution",
                    endDateTime, ts.TotalDays, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds), "TimerEvent");
            }
        }
        /// <summary>
        /// To check whether specified time reached to run the service
        /// </summary>
        /// <param name="endpointElement"></param>
        /// <returns></returns>
        private bool isScheduledTime(ConnectionManagerEndpointElement endpointElement)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan sceduledtime;
            TimeSpan.TryParse(endpointElement.ScheduledTime, out sceduledtime);
            return (DateTime.Now.TimeOfDay >= sceduledtime && (DateTime.Now.ToString("dd/MM/yyyy") != endpointElement.LastRunEndDateTime.ToString("dd/MM/yyyy")));
            
        }



        /// <summary>
        /// Launching the specified service for execution
        /// </summary>
        /// <param name="wcServiceElement"></param>
        private void LaunchWCService(ConnectionManagerEndpointElement wcServiceElement)
        {
            bool executeStatus = false;
            WCScheduledProcessExecute wcScheduledProcessExecute = new WCScheduledProcessExecute();
            WriteEventLog(wcServiceElement.ServiceName + " started at {0}");
            try
            {
                wcServiceElement.LastRunStartDateTime = DateTime.Now;

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["RecordProcessStatus"]))
                {
                    WriteEventLog(string.Format("{0} service is started at {1}", wcServiceElement.ServiceName, wcServiceElement.LastRunStartDateTime),
                        wcServiceElement.ServiceName);
                }

                wcScheduledProcessExecute.Execute(wcServiceElement.ServiceName);

                executeStatus = true;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0}: {1}", "Message", ex.Message));
                sb.AppendLine(string.Format("{0}: {1}", "StackTrace", ex.StackTrace));
                WriteEventLog(sb.ToString(), wcServiceElement.ServiceName);
            }
            finally
            {
                wcScheduledProcessExecute = null;

                try
                {
                    wcServiceElement.LastRunEndDateTime = DateTime.Now;
                    wcServiceElement.IsServiceRun = true;
                }
                catch
                {
                }
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["RecordProcessStatus"]))
            {
                TimeSpan ts = wcServiceElement.LastRunEndDateTime - wcServiceElement.LastRunStartDateTime;
                WriteEventLog(
                    string.Format("{0} service is {1} at {2} and taken time for {3} Days {4} hours {5} minutes {6} seconds and {7} milliseconds for execution",
                    wcServiceElement.ServiceName, wcServiceElement.LastRunEndDateTime,
                    (executeStatus ? "Completed successfully" : "Failed (see previous entry for failure reason)"),
                    ts.TotalDays, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds), wcServiceElement.ServiceName);
            }
        }

        /// <summary>
        /// Write log into file
        /// </summary>
        /// <param name="text"></param>
        private void WriteEventLog(string text)
        {
            BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            //string path = "D:\\EventLog.txt";
            //using (StreamWriter writer = new StreamWriter(path, true))
            //{
            //    writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
            //    writer.Close();
            //}
        }

        /// <summary>
        /// Writing in Event log and change source on the fly.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        private void WriteEventLog(string message, string source)
        {
            BHIC.Common.Logging.LoggingService.Instance.Trace(string.Format("{0} : {1}", source, message));
            //string path = "D:\\EventLog.txt";
            //using (StreamWriter writer = new StreamWriter(path, true))
            //{
            //    writer.WriteLine("{0} : {1}", source, message);
            //    writer.Close();
            //}
        }

        #endregion

        #endregion
    }
}
