#region Using direcitves

using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;

using BHIC.Common.Caching;
using BHIC.Common.Config;
using BHIC.Contract.PurchasePath;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.DML.WC.DTO;
using BHIC.Common.XmlHelper;
using BHIC.Common;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class MultiStateService : IMultiStateService
    {
        #region Variables

        CacheHelper cache;

        #endregion

        #region Methods

        #region Constructor

        public MultiStateService()
        {
            cache = CacheHelper.Instance;
        }

        #endregion

        #region Interface implementation

        /// <summary>
        /// Get list of states from database
        /// </summary>
        /// <returns></returns>
        List<ZipCodeStates> IMultiStateService.GetStates()
        {
            var multiStates = new List<ZipCodeStates>();

            if (cache.Get<List<ZipCodeStates>>(Constants.MultipleStates) != null)
            {
                //fetch state list from cache
                multiStates = cache.Get<List<ZipCodeStates>>(Constants.MultipleStates).ToList();
            }
            else
            {
                //if state list not exits, create new in cache
                if (SetMultipleStates())
                {
                    //on success fetch state list from cache
                    multiStates = cache.Get<List<ZipCodeStates>>(Constants.MultipleStates).ToList();
                }
            }

            return multiStates;
        }

        /// <summary>
        /// Set list of states into cache
        /// </summary>
        /// <returns></returns>
        bool IMultiStateService.SetStatesCache()
        {
            try
            {
                SetMultipleStates();
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
        /// It will store multiple states into cache
        /// </summary>
        /// <returns>Return true on success, false otherwise</returns>
        private bool SetMultipleStates()
        {
            IMultipleStateDataProvider multipleStates = new MultipleStateDataProvider();
            var states = new List<ZipCodeStates>();

            try
            {
                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //string serviceName = string.Empty;
                //serviceName = "ListOfMultipleStates";
                //string filePath = System.IO.Path.Combine(ConfigCommonKeyReader.LocalStoredDataPath, string.Concat(serviceName, ".txt"));

                ////In case want to reload local stored data then set 

                //if (ConfigCommonKeyReader.IsDevEnvironment && !ConfigCommonKeyReader.RelaodLocalData)
                //{
                //    var getLocalData = UtilityFunctions.ReadTxtData(serviceName);

                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (!string.IsNullOrEmpty(getLocalData))
                //        states = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ZipCodeStates>>(getLocalData);
                //}

                //#endregion

                //Comment : Here if list already having records then skip below DB call
                if (!(states.Count > 0))
                    states = multipleStates.GetStatesByZipCode();

                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ConfigCommonKeyReader.MultipleStatesCacheInterval);

                if (cache.IsExists(Constants.MultipleStates))
                {
                    cache.Remove(Constants.MultipleStates);
                }

                //add item into cache
                cache.Add(Constants.MultipleStates, states, policy);

                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //if (ConfigCommonKeyReader.IsDevEnvironment)
                //{
                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (states.Count > 0)
                //    {
                //        UtilityFunctions.WriteTxtData(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(states));
                //    }
                //}

                //#endregion
            }
            catch
            {
                throw;
            }

            return states.Count > 0 ? true : false;
        }

        #endregion

        #endregion
    }
}
