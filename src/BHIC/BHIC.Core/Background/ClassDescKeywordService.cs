using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Common.Config;
using BHIC.Common.Client;
using BHIC.Common;
using BHIC.Common.Caching;
using System.Runtime.Caching;

namespace BHIC.Core.Masters
{
    public class ClassDescKeywordService : IClassDescKeywordService
    {
        #region Methods
        CacheHelper cache;

        #region Public Methods

        public ClassDescKeywordService()
        {
            cache = CacheHelper.Instance;
        }

        /// <summary>
        /// Returns list of ClassDescriptionKeywords based on supplied ClassDescKeywordId or SeacrhString
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private List<ClassDescriptionKeyword> GetClassDescKeywordList(ClassDescKeywordRequestParms args, ServiceProvider provider)
        {
            var classDescKeywordResponse = SvcClient.CallService<ClassDescKeywordResponse>
                                            (string.Concat(Constants.ServiceNames.ClassDescKeywords, UtilityFunctions.CreateQueryString<ClassDescKeywordRequestParms>(args)), provider);

            if (classDescKeywordResponse.OperationStatus.RequestSuccessful)
            {
                return classDescKeywordResponse.ClassDescKeywords;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(classDescKeywordResponse.OperationStatus.Messages));
            }
        }


        public List<ClassDescriptionKeyword> GetClassDescKeywordList(ClassDescKeywordRequestParms args, ServiceProvider provider, bool fromCache)
        {
            List<ClassDescriptionKeyword> filteredData = new List<ClassDescriptionKeyword>();
            if (fromCache)
            {
                if (cache.Get<List<ClassDescriptionKeyword>>(Constants.ClassDescriptionListCache).IsNull())
                {
                    SetClassDescKeywordListCache();
                }
                filteredData = cache.Get<List<ClassDescriptionKeyword>>(Constants.ClassDescriptionListCache).ToList();
                return filteredData.Where(x => x.Keyword.Contains(args.SearchString)).ToList();
            }
            else
                return GetClassDescKeywordList(args, provider);
        }
        /// <summary>
        /// Returns list of ClassDescriptionKeywords based on supplied ClassDescKeywordId or SeacrhString
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public void SetClassDescKeywordListCache()
        {
            List<ClassDescriptionKeyword> resp = new List<ClassDescriptionKeyword>();
            ServiceProvider provider = new GuardServiceProvider();
            ClassDescKeywordRequestParms args = new ClassDescKeywordRequestParms() { SearchString = string.Empty, LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC) };
            resp = GetClassDescKeywordList(args, provider);
            //add cache item policy
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(1);

            //add item into cache
            cache.Add(Constants.ClassDescriptionListCache, resp, policy);
        }
        #endregion

        #endregion
    }
}
