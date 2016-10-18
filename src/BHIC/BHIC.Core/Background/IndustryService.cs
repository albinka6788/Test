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
using BHIC.Common.Caching;
using BHIC.Common.Client;
using System.Runtime.Caching;
using BHIC.Common.XmlHelper;

namespace BHIC.Core
{
    public class IndustryService : IIndustryService
    {
        CacheHelper cache;
        public IndustryService()
        {
            cache = CacheHelper.Instance;
        }
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return list of Industries based on LOB or IndustryID
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Industry> GetIndustryList(IndustryRequestParms args, ServiceProvider provider)
        {
            var industryResponse = new IndustryResponse();
            if (!(cache.Get<List<Industry>>(Constants.IndustryCache).IsNull()))
            {
                industryResponse.Industries = cache.Get<List<Industry>>(Constants.IndustryCache).ToList();
            }
            else
            {
                industryResponse.Industries = GetIndustries(args, provider);
            }
            return industryResponse.Industries;
        }

        /// <summary>
        /// Return specific Industry detail based on industryId
        /// </summary>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public Industry GetIndustry(int industryId, ServiceProvider provider)
        {
            var industryResponse = SvcClient.CallService<IndustryResponse>(string.Concat(Constants.ServiceNames.Industries, UtilityFunctions.CreateQueryString<IndustryRequestParms>(
                                    new IndustryRequestParms() { IndustryId = industryId })), provider);

            //Comment : Here if get data then return Zero index industry from list of industries
            if (industryResponse.OperationStatus.RequestSuccessful)
            {
                var industry = industryResponse.Industries.SingleOrDefault(i => i.IndustryId == industryId);

                if (industry != null)
                {
                    return industry;
                }
                else
                {
                    throw new ApplicationException(string.Format("Industry with id {0} does not found", industryId));
                }
            }
            else
            {
                throw new ApplicationException(industryResponse.OperationStatus.Messages.Single().ToString());
            }
        }

        /// <summary>
        /// Set list of industries into cache
        /// </summary>
        /// <returns></returns>
        bool IIndustryService.SetIndustryCache(IndustryRequestParms args, ServiceProvider provider)
        {
            try
            {
                GetIndustries(args,provider);
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
        /// Sets industries on the  basis of Request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool SetIndustries(IndustryResponse industryResponse)
        {
            try
            {
                //add cache item policy
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ConfigCommonKeyReader.IndustryCacheInterval);

                if (cache.IsExists(Constants.IndustryCache))
                {
                    cache.Remove(Constants.IndustryCache);
                }

                //add item into cache
                cache.Add(Constants.IndustryCache, industryResponse.Industries, policy);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the Industry(s) specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <param name="provider"></param>
        /// <returns>List of County</returns>
        private List<Industry> GetIndustries(IndustryRequestParms args, ServiceProvider provider)
        {
            var industryResponse = SvcClient.CallService<IndustryResponse>(string.Concat(Constants.ServiceNames.Industries, BHIC.Common.UtilityFunctions.CreateQueryString<IndustryRequestParms>(args)), provider);

            if (industryResponse.OperationStatus.RequestSuccessful)
            {
                SetIndustries(industryResponse);
                return industryResponse.Industries;
            }
            else
            {
                throw new ApplicationException(BHIC.Common.UtilityFunctions.ConvertMessagesToString(industryResponse.OperationStatus.Messages));
            }
        }

        #endregion Private Methods

        #endregion
    }
}
