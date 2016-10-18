using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Common.Client;

namespace BHIC.Contract.Background
{
    public interface IClassDescKeywordService
    {
        /// <summary>
        /// Returns list of ClassDescriptions based on supplied ClassDescKeywordId or SeacrhString
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<ClassDescriptionKeyword> GetClassDescKeywordList(ClassDescKeywordRequestParms args, ServiceProvider provider, bool fromCache);

        /// <summary>
        /// Returns list of ClassDescriptions based on supplied ClassDescKeywordId or SeacrhString
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        void SetClassDescKeywordListCache();
    }
}
