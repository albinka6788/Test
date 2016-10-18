#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using BHIC.Common.Caching;
using BHIC.Common.Config;
using BHIC.DML.WC.DTO;
using BHIC.Contract.PurchasePath;
using System.Runtime.Caching;
using BHIC.DML.WC.DataContract;
using BHIC.Common.XmlHelper;
using BHIC.Common;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class StateTypeService : IStateTypeService
    {
        #region Variables

        CacheHelper cache;

        #endregion

        #region Constructor

        public StateTypeService()
        {
            cache = CacheHelper.Instance;
        }

        #endregion

        #region Methods

        #region Interface implementation

        /// <summary>
        /// Get List of Good and bad states
        /// </summary>
        /// <returns>Returns all good and bad states</returns>
        List<StateType> IStateTypeService.GetAllGoodAndBadState()
        {
            var states = new List<StateType>();

            if (cache.Get<List<StateType>>(Constants.StateTypeCache) != null)
            {
                //fetch state list from cache
                states = cache.Get<List<StateType>>(Constants.StateTypeCache).ToList();
            }
            else
            {
                //if state list not exits, create new in cache
                if (SetGoodAndBadState())
                {
                    //on success fetch state list from cache
                    states = cache.Get<List<StateType>>(Constants.StateTypeCache).ToList();
                }
            }

            return states;
        }

        /// <summary>
        /// Set List of Good and bad states into cache
        /// </summary>
        /// <returns></returns>
        bool IStateTypeService.SetAllGoodAndBadStateCache()
        {
            try
            {
                SetGoodAndBadState();
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
        /// It will store all good and bad state list  into cache
        /// </summary>
        /// <returns></returns>
        private bool SetGoodAndBadState()
        {
            ICaptureQuote captureQuote = new CaptureQuote();
            List<StateType> state = new List<StateType>();

            try
            {
                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //string serviceName = string.Empty;
                //serviceName = "ListOfGoodAndBadState";
                //string filePath = System.IO.Path.Combine(ConfigCommonKeyReader.LocalStoredDataPath, string.Concat(serviceName, ".txt"));

                ////In case want to reload local stored data then set 

                //if (ConfigCommonKeyReader.IsDevEnvironment && !ConfigCommonKeyReader.RelaodLocalData)
                //{
                //    var getLocalData = UtilityFunctions.ReadTxtData(serviceName);

                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (!string.IsNullOrEmpty(getLocalData))
                //        state = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StateType>>(getLocalData);
                //}

                //#endregion

                //Comment : Here if list already having records then skip below DB call
                if (!(state.Count > 0))
                    state = captureQuote.GetListOfGoodAndBadState();

                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ConfigCommonKeyReader.GoodAndBadStateCacheInterval);

                if (cache.IsExists(Constants.StateTypeCache))
                {
                    cache.Remove(Constants.StateTypeCache);
                }

                //add item into cache
                cache.Add(Constants.StateTypeCache, state, policy);

                //#region Comment : Here Only on "Development" environmnet check for local stored copy to reduce app loading time

                //if (ConfigCommonKeyReader.IsDevEnvironment)
                //{
                //    //Comment : Here app able to get local stored data then by-pass further below processing 
                //    if (state.Count > 0)
                //    {
                //        UtilityFunctions.WriteTxtData(filePath, Newtonsoft.Json.JsonConvert.SerializeObject(state));
                //    }
                //}

                //#endregion
            }
            catch (Exception)
            {

            }

            return state.Count > 0 ? true : false;
        }

        #endregion

        #endregion
    }
}
