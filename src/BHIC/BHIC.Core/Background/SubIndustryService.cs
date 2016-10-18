using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common;
using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Domain.Service;
using BHIC.Common.Config;
using BHIC.Common.Client;
using BHIC.Common.Caching;
using System.Runtime.Caching;
using BHIC.Common.XmlHelper;

namespace BHIC.Core
{
    public class SubIndustryService : ISubIndustryService
    {
        #region Methods
        CacheHelper cache;
        public SubIndustryService()
        {
            cache = CacheHelper.Instance;
        }
        #region Public Methods

        /// <summary>
        /// Return specific SubIndustry detail based on industryId 
        /// </summary>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public SubIndustry GetSubIndustry(int subIndustryId, ServiceProvider Provider)
        {
            var subIndustryResponse = SvcClient.CallService<SubIndustryResponse>(string.Concat(Constants.ServiceNames.SubIndustries, UtilityFunctions.CreateQueryString<SubIndustryRequestParms>(
                                    new SubIndustryRequestParms() { SubIndustryId = subIndustryId })), Provider);

            //Comment : Here if get data then return Zero index industry from list of industries
            if (subIndustryResponse.OperationStatus.RequestSuccessful)
            {
                var subIndustry = subIndustryResponse.SubIndustries.SingleOrDefault(i => i.SubIndustryId == subIndustryId);

                if (!subIndustry.IsNull())
                {
                    return subIndustry;
                }
                else
                {
                    throw new ApplicationException(string.Format("SubIndustry with id {0} does not found", subIndustryId));
                }
            }
            else
            {
                throw new ApplicationException(subIndustryResponse.OperationStatus.Messages.Single().ToString());
            }
        }

        /// <summary>
        /// Return list of Industries based on LOB or IndustryID
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<SubIndustry> GetSubIndustryList(SubIndustryRequestParms args, ServiceProvider provider)
        {
            var subIndustryResponse = new SubIndustryResponse();
            if (!(cache.Get<List<SubIndustry>>(Constants.SubIndustryCache).IsNull()))
            {
                subIndustryResponse.SubIndustries = cache.Get<List<SubIndustry>>(Constants.SubIndustryCache).ToList();
            }
            else
            {
                subIndustryResponse.SubIndustries = GetSubIndustries(args, provider);
            }
            return subIndustryResponse.SubIndustries;
        }


        /// <summary>
        /// Set list of sub-industries into cache
        /// </summary>
        /// <returns></returns>
        bool ISubIndustryService.SetSubIndustryCache(SubIndustryRequestParms args, ServiceProvider provider)
        {
            try
            {
                GetSubIndustries(args, provider);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        /// <summary>
        /// Get the Industry(s) specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <param name="provider"></param>
        /// <returns>List of County</returns>
        private List<SubIndustry> GetSubIndustries(SubIndustryRequestParms args, ServiceProvider provider)
        {
            var subIndustryResponse = SvcClient.CallService<SubIndustryResponse>(string.Concat(Constants.ServiceNames.SubIndustries, BHIC.Common.UtilityFunctions.CreateQueryString<SubIndustryRequestParms>(args)), provider);

            if (subIndustryResponse.OperationStatus.RequestSuccessful)
            {
                SetSubIndustries(subIndustryResponse);
                return subIndustryResponse.SubIndustries;
            }
            else
            {
                throw new ApplicationException(BHIC.Common.UtilityFunctions.ConvertMessagesToString(subIndustryResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Sets industries on the  basis of Request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool SetSubIndustries(SubIndustryResponse subIndustryResponse)
        {
            try
            {
                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ConfigCommonKeyReader.SubIndustryCacheInterval);

                if (cache.IsExists(Constants.SubIndustryCache))
                {
                    cache.Remove(Constants.SubIndustryCache);
                }

                //add item into cache
                cache.Add(Constants.SubIndustryCache, subIndustryResponse.SubIndustries, policy);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}
