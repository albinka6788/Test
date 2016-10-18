#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IOfficerService
    {
        /// <summary>
        /// Returns list of Officer based on QuoteId and OfficerId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Officer> GetOfficerList(OfficerRequestParms args);

        /// <summary>
        /// Save new Officer details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddOfficer(Officer args);

        /// <summary>
        /// Delete existing Officer details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteOfficer(OfficerRequestParms args);
    }
}
