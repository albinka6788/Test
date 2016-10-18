#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

using BHIC.Common.Caching;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Background;
using BHIC.Contract.Provider;
using BHIC.Domain.Background;
using System.Threading;
using BHIC.Common.XmlHelper;

#endregion

namespace BHIC.Core.Background
{
    public class CountyService : IServiceProviders, ICountyService
    {
        #region Variables

        CacheHelper cache;
        private readonly object cacheLock = new object();

        #endregion

        #region Constructor

        public CountyService(ServiceProvider provider)
        {
            cache = CacheHelper.Instance;
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Interface implementation

        /// <summary>
        /// Returns county list from cache
        /// </summary>
        /// <returns>List of county</returns>
        IList<County> ICountyService.GetCounty(bool isThreadMode)
        {
            var county = new List<County>();

            if (cache.Get<List<County>>(Constants.CountyCache) != null)
            {
                //fetch county list from cache
                county = cache.Get<List<County>>(Constants.CountyCache).ToList().ConvertAll(res =>
                        new County { City = res.City, FIPS = res.FIPS, Name = res.Name, State = res.State, ZipCode = res.ZipCode });
            }
            else
            {
                //if county list not exits, create new in cache
                //if isThreadMode is true, run function in thread mode
                if (isThreadMode)
                {
                    Thread thread = new Thread(new ThreadStart(SetCounty));
                    thread.Start();

                }
                else
                {
                    SetCounty();
                }

                //on success fetch county list from cache
                county = cache.Get<List<County>>(Constants.CountyCache).ConvertAll(res =>
                        new County { City = res.City, FIPS = res.FIPS, Name = res.Name, State = res.State, ZipCode = res.ZipCode }); ;
            }
            return county;
        }

        /// <summary>
        /// Set county list into cache
        /// </summary>
        /// <returns></returns>
        bool ICountyService.SetCountyCache()
        {
            try
            {
                SetCache();
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
        /// Store county into cache
        /// </summary>
        /// <returns>true on success, false otherwise</returns>
        private void SetCounty()
        {
            if (cache.Get<List<County>>(Constants.CountyCache) == null)
            {
                try
                {
                    SetCache();
                }
                catch (Exception ex)
                {
                    //log exception
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get the County(s) specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <param name="provider"></param>
        /// <returns>List of County</returns>
        private List<County> GetCountyList(CountyRequestParms args)
        {
            var countyResponse = SvcClient.CallService<CountyResponse>(string.Concat(Constants.County,
                BHIC.Common.UtilityFunctions.CreateQueryString<CountyRequestParms>(args)), ServiceProvider);

            if (countyResponse.OperationStatus.RequestSuccessful)
            {
                return countyResponse.Counties;
            }
            else
            {
                throw new ApplicationException(BHIC.Common.UtilityFunctions.ConvertMessagesToString(countyResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Set county list into cache
        /// </summary>
        private void SetCache()
        {
            //fetch cache list from county api service
            var county = this.GetCountyList(new CountyRequestParms { ZipCode = string.Empty });

            //add cache item policy
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Convert.ToDouble(ConfigCommonKeyReader.CountyCacheInterval));

            if (cache.IsExists(Constants.CountyCache))
            {
                cache.Remove(Constants.CountyCache);
            }

            //add item into cache
            cache.Add(Constants.CountyCache, county, policy);
        }

        #endregion

        #endregion
    }
}
