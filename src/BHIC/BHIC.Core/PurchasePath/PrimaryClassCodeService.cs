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

#endregion

namespace BHIC.Core.PurchasePath
{
    public class PrimaryClassCodeService : IPrimaryClassCodeService
    {
        #region Variables

        CacheHelper cache;

        #endregion

        #region Constructor

        public PrimaryClassCodeService()
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
        List<PrimaryClassCodeDTO> IPrimaryClassCodeService.GetPrimaryClassCodeDataList()
        {
            var lob = new List<PrimaryClassCodeDTO>();

            if (cache.Get<List<PrimaryClassCodeDTO>>(Constants.PrimaryClassCodeCache) != null)
            {
                //fetch lob list from cache
                lob = cache.Get<List<PrimaryClassCodeDTO>>(Constants.PrimaryClassCodeCache).ToList();
            }
            else
            {
                //if lob list not exits, create new in cache
                if (SetPrimaryClassCodeDataList())
                {
                    //on success fetch lob list from cache
                    lob = cache.Get<List<PrimaryClassCodeDTO>>(Constants.PrimaryClassCodeCache).ToList();
                }
            }

            return lob;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Store all line of business into cache
        /// </summary>
        /// <returns></returns>
        private bool SetPrimaryClassCodeDataList()
        {
            IPrimaryClassCodeDataProvider lob = new PrimaryClassCodeDataProvider();
            List<PrimaryClassCodeDTO> lobList = new List<PrimaryClassCodeDTO>();

            try
            {
                lobList = lob.GetAllPrimaryClassCodeData();

                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(Constants.CountyCacheDuration);

                //add item into cache
                cache.Add(Constants.PrimaryClassCodeCache, lobList, policy);
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
