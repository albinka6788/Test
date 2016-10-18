#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;
using BHIC.Domain.Policy;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IVInsuredNameFEINService
    {
        /// <summary>
        /// Returns FEIN number validation operation status 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus ValidateFeinNumber(VInsuredNameFEINRequestParms args);
    }
}
