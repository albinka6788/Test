#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IInsuredNameService
    {
        /// <summary>
        /// Returns list of InsuredName based on QuoteId and InsuredNameId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<InsuredName> GetInsuredNameList(InsuredNameRequestParms args);

        /// <summary>
        /// Save new InsuredName details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddInsuredName(InsuredName args);

        /// <summary>
        /// Delete existing InsuredName details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteInsuredName(InsuredNameRequestParms args);

    }
}
