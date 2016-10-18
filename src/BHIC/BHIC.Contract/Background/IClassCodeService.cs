using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;

namespace BHIC.Contract.Background
{
    public interface IClassCodeService
    {
        /// <summary>
        /// Returns list of Class codes based on StateAbbr, Class Code, ClassSuffix
        /// </summary>
        /// <returns></returns>
        List<ClassCode> GetClassCodeList(ClassCodeRequestParms args);
    }
}
