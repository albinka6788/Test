#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Contract.Provider;

#endregion

namespace BHIC.Core.Policy
{
    public class CancellationRequestService : IServiceProviders, ICancellationRequestService
    {
        #region Methods

        #region Public Methods

        public CancellationRequestService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        /// <summary>
        /// Returns list of Cancellation request based on QuoteId,PolicyId and CancellationRequestId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<CancellationRequest> GetCancellationRequestList(CancellationRequestParms args)
        {
            var cancellationRequestResponse = SvcClient.CallService<CancellationRequestResponse>(string.Concat(Constants.CancellationRequest,
                UtilityFunctions.CreateQueryString<CancellationRequestParms>(args)), ServiceProvider);

            if (cancellationRequestResponse.OperationStatus.RequestSuccessful)
            {
                return cancellationRequestResponse.CancellationRequests;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(cancellationRequestResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new CancellationRequest details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddCancellationRequest(CancellationRequest args)
        {
            var cancellationRequestResponse = SvcClient.CallServicePost<CancellationRequest, OperationStatus>(Constants.CancellationRequest, args, ServiceProvider);
            if (cancellationRequestResponse.RequestSuccessful)
            {
                return cancellationRequestResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(cancellationRequestResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
