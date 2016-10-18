#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Domain.Policy;
using BHIC.Domain.Service;

#endregion

namespace BHIC.Contract.Policy
{
    public interface ICertRequestService
    {
        /// <summary>
        /// Returns list of Cert Request based on QuoteId,PolicyId and CertRequestId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<CertRequest> GetCertRequestList(CertRequestParms args);

        /// <summary>
        /// Save new Cert Request details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddCertRequest(CertRequest args);
    }
}
