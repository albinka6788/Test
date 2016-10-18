#region Using directives

using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Common.WCScheduleProcess
{
    class BHICReloadCache : BackGroundProcess
    {
        #region Methods

        #region Public methods

        /// <summary>
        /// Override base class method to implement functionality to call Cache Classes
        /// </summary>
        /// <returns></returns>
        public override bool Process()
        {
            bool retValue = true;
            this.BackgroundTaskID = BackgroundTaskListItem.ReloadCache;
            this.ProcessStartDateTime = DateTime.Now;

            List<string> cacheServiceNames = new List<string>();

            cacheServiceNames.Add(Constants.CountyCache);
            cacheServiceNames.Add(Constants.IndustryCache);
            cacheServiceNames.Add(Constants.SubIndustryCache);
            cacheServiceNames.Add(Constants.LineOfBusinessCache);
            cacheServiceNames.Add(Constants.StateTypeCache);
            cacheServiceNames.Add(Constants.MultipleStates);
            
            try
            {
                if (!string.IsNullOrEmpty(ConfigCommonKeyReader.RefreshCache))
                {
                    bool result = false;
                    int attempt;

                    foreach (var item in cacheServiceNames)
                    {
                        attempt = 0;
                        //if process does not executed successfully, try to re-attempt
                        //if no. of attempts exceeded more than 5 times, terminate processs
                        do
                        {
                            var encryptedText = Encryption.EncryptText(string.Format("cacheServiceName={0}&expiryTime={1}", item, DateTime.Now.AddMinutes(2)));

                            var urlPath = string.Format(ConfigCommonKeyReader.RefreshCache, ConfigCommonKeyReader.HostURL, encryptedText);

                            //de-serialize json result 
                            result = UtilityFunctions.GetJsonResult(URL.ReadAllText(urlPath));

                            attempt++;

                        } while (!result && attempt < 5);

                    }
                    this.LogProcessStatus(BackgroundTaskStatus.Completed, "Reload Cache Process Completed");
                }
                else
                {
                    this.LogProcessStatus(BackgroundTaskStatus.Completed, "Reload Cache Process Not run as Reload Cache URL not defined");
                }
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
