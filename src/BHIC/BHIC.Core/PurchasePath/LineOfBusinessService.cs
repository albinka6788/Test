#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

using BHIC.Common.Caching;
using BHIC.Common.Config;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Contract.PurchasePath;
using BHIC.Common.XmlHelper;
using BHIC.Common;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class LineOfBusinessService : ILineOfBusinessService
    {
        #region Variables

        CacheHelper cache;

        #endregion

        #region Constructor

        public LineOfBusinessService()
        {
            cache = CacheHelper.Instance;
        }

        #endregion

        #region Methods

        #region Interface implementation

        /// <summary>
        /// Get List of all Line of business
        /// </summary>
        /// <returns>Returns list of LineOfBusiness from cache</returns>
        List<LineOfBusiness> ILineOfBusinessService.GetLineOfBusiness()
        {
            var lob = new List<LineOfBusiness>();

            if (cache.Get<List<LineOfBusiness>>(Constants.LineOfBusinessCache) != null)
            {
                //fetch lob list from cache
                lob = cache.Get<List<LineOfBusiness>>(Constants.LineOfBusinessCache).ToList();
            }
            else
            {
                //if lob list not exits, create new in cache
                if (SetLineOfBusiness())
                {
                    //on success fetch lob list from cache
                    lob = cache.Get<List<LineOfBusiness>>(Constants.LineOfBusinessCache).ToList();
                }
            }

            return lob;
        }

        /// <summary>
        /// Set LineOfBusiness list into cache
        /// </summary>
        /// <returns></returns>
        bool ILineOfBusinessService.SetLineOfBusinessCache()
        {
            try
            {
                SetLineOfBusiness();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Store all line of business into cache
        /// </summary>
        /// <returns></returns>
        private bool SetLineOfBusiness()
        {
            ILineOfBusinessProvider lob = new LineOfBusinessProvider();
            List<LineOfBusiness> lobList = new List<LineOfBusiness>();

            try
            {
                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //string serviceName = string.Empty;
                //serviceName = "LineOfBusiness";
                //string filePath = System.IO.Path.Combine(ConfigCommonKeyReader.LocalStoredDataPath, string.Concat(serviceName, ".txt"));

                ////In case want to reload local stored data then set 

                //if (ConfigCommonKeyReader.IsDevEnvironment && !ConfigCommonKeyReader.RelaodLocalData)
                //{
                //    var getLocalData = UtilityFunctions.ReadTxtData(serviceName);

                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (!string.IsNullOrEmpty(getLocalData))
                //        lobList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LineOfBusiness>>(getLocalData);
                //}

                //#endregion

                //Comment : Here if list already having records then skip below DB call
                if (!(lobList.Count > 0))
                    lobList = lob.GetAllLineOfBusiness();

                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ConfigCommonKeyReader.LineOfBusinessCacheInterval);

                if (cache.IsExists(Constants.LineOfBusinessCache))
                {
                    cache.Remove(Constants.LineOfBusinessCache);
                }

                //add item into cache
                cache.Add(Constants.LineOfBusinessCache, lobList, policy);

                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //if (ConfigCommonKeyReader.IsDevEnvironment)
                //{
                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (lobList.Count > 0)
                //    {
                //        UtilityFunctions.WriteTxtData(filePath,Newtonsoft.Json.JsonConvert.SerializeObject(lobList));
                //    }
                //}

                //#endregion
            }
            catch (Exception)
            {

            }

            return lobList.Count > 0 ? true : false;
        }

        #endregion

        #endregion
    }
}
