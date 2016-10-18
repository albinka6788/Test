using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.Background
{
    public interface ICompanionClassService
    {
        /// <summary>
        /// Get Companion Classes
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<CompanionClass> GetCompanionClasses(CompanionClassRequestParms args);
    }
}
