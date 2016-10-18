#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;

#endregion

namespace BHIC.Core.Policy
{
    class CertRequestService : ICertRequestService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Cert Request based on QuoteId,PolicyId and CertRequestId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<CertRequest> GetCertRequestList(CertRequestParms args)
        {
            var CertRequestResponse = SvcClientOld.CallService<CertRequestResponse>(string.Concat(Constants.CertRequest,
                UtilityFunctions.CreateQueryString<CertRequestParms>(args)));

            if (CertRequestResponse.OperationStatus.RequestSuccessful)
            {
                return CertRequestResponse.CertRequests;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(CertRequestResponse.OperationStatus.Messages));
            }

        }

        /// <summary>
        /// Save new Cert Request details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddCertRequest(CertRequest args)
        {
            var certRequestResponse = SvcClientOld.CallService<CertRequest, OperationStatus>(Constants.CertRequest, "POST", args);

            if (certRequestResponse.RequestSuccessful)
            {
                return certRequestResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(certRequestResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
