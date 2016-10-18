#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface ICancellationRequestService
    {
        /// <summary>
        /// Returns list of Cancellation request based on QuoteId,PolicyId and CancellationRequestId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<CancellationRequest> GetCancellationRequestList(CancellationRequestParms args);

        /// <summary>
        /// Save new CancellationRequest details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddCancellationRequest(CancellationRequest args);
    }
}
