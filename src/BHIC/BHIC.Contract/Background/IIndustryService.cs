using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Common.Client;

namespace BHIC.Contract.Background
{
    public interface IIndustryService
    {
        /// <summary>
        /// Return list of industries based on Lob or IndustryId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Industry> GetIndustryList(IndustryRequestParms args, ServiceProvider provider);

        /// <summary>
        /// Return specific industry description detail based on supplied IndustryId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Industry GetIndustry(int industryId, ServiceProvider provider);

        /// <summary>
        /// Set list of industries into cache
        /// </summary>
        /// <returns></returns>
        bool SetIndustryCache(IndustryRequestParms args, ServiceProvider provider);
    }
}
