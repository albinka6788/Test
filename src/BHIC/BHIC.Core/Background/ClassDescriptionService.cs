using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Contract.Background;
using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Common.Client;
using BHIC.Common.Caching;
using System.Runtime.Caching;

namespace BHIC.Core.Masters
{
    public class ClassDescriptionService : IClassDescriptionService
    {
        #region Methods

        CacheHelper cache;
        public ClassDescriptionService()
        {
            cache = CacheHelper.Instance;
        }
        #region Public Methods

        /// <summary>
        /// Return list of classdescriptions based on Lob, SubIndustryId and ClassDescriptionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<ClassDescription> GetClassDescriptionList(ClassDescriptionRequestParms args, ServiceProvider Provider)
        {
            var classDescriptionResponse = new ClassDescriptionResponse();
            if (!(cache.Get<List<ClassDescription>>(Constants.ClassCache).IsNull()))
            {
                classDescriptionResponse.ClassDescriptions = cache.Get<List<ClassDescription>>(Constants.ClassCache).ToList();
            }
            else
            {
                classDescriptionResponse.ClassDescriptions = GetClassDescriptions(args, Provider);
            }

            classDescriptionResponse.ClassDescriptions.ForEach(res => res.CompanionClasses = res.CompanionClasses.Where(p => !string.IsNullOrWhiteSpace(p.PromptText)).ToList());

            return classDescriptionResponse.ClassDescriptions;
        }

        /// <summary>
        /// Return specific ClassDescription detail based on ClassDescriptionId
        /// </summary>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        public ClassDescription GetClassDescription(int classDescriptionId, ServiceProvider Provider)
        {
            var classDescResponse = SvcClient.CallService<ClassDescriptionResponse>(string.Concat(Constants.ServiceNames.ClassDescriptions, UtilityFunctions.CreateQueryString<ClassDescriptionRequestParms>(
                                    new ClassDescriptionRequestParms() { IncludeRelated = true, ClassDescriptionId = classDescriptionId })), Provider);

            //Comment : Here if get data then return Zero index industry from list of industries
            if (classDescResponse.OperationStatus.RequestSuccessful)
            {
                var classDescription = classDescResponse.ClassDescriptions.SingleOrDefault(i => i.ClassDescriptionId == classDescriptionId);

                if (classDescription != null)
                {
                    return classDescription;
                }
                else
                {
                    throw new ApplicationException(string.Format("ClassDescription with id {0} does not found", classDescriptionId));
                }
            }
            else
            {
                throw new ApplicationException(classDescResponse.OperationStatus.Messages.Single().ToString());
            }
        }

        /// <summary>
        /// Get the Industry(s) specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <param name="provider"></param>
        /// <returns>List of County</returns>
        private List<ClassDescription> GetClassDescriptions(ClassDescriptionRequestParms args, ServiceProvider provider)
        {
            var classDescriptionResponse = SvcClient.CallService<ClassDescriptionResponse>(string.Concat(Constants.ServiceNames.ClassDescriptions, BHIC.Common.UtilityFunctions.CreateQueryString<ClassDescriptionRequestParms>(args)), provider);

            if (classDescriptionResponse.OperationStatus.RequestSuccessful)
            {
                //SetClassDescriptions(classDescriptionResponse);
                return classDescriptionResponse.ClassDescriptions;
            }
            else
            {
                throw new ApplicationException(BHIC.Common.UtilityFunctions.ConvertMessagesToString(classDescriptionResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Sets industries on the  basis of Request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool SetClassDescriptions(ClassDescriptionResponse ClassDescriptionResponse)
        {
            if (cache.Get<List<ClassDescription>>(Constants.ClassCache).IsNull())
            {
                try
                {
                    //add cache item policy
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);

                    //add item into cache
                    cache.Add(Constants.ClassCache, ClassDescriptionResponse.ClassDescriptions, policy);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}
