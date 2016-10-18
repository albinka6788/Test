using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Common.Client;

namespace BHIC.Contract.Background
{
    public interface ISubIndustryService
    {
        /// <summary>
        /// Return list of sub-industries based on Lob or IndustryId or SubIndustryId
        /// </summary>
        /// <returns></returns>
        List<SubIndustry> GetSubIndustryList(SubIndustryRequestParms args, ServiceProvider provider);

        /// <summary>
        /// Return specific sub-industry detail based on supplied SubIndustryId
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        SubIndustry GetSubIndustry(int subIndustryId, ServiceProvider provider);

        /// <summary>
        /// Set list of sub-industries into cache
        /// </summary>
        /// <returns></returns>
        bool SetSubIndustryCache(SubIndustryRequestParms args, ServiceProvider provider);
    }
}
