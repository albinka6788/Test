#region Using directives
using System;
using System.Collections.Generic;

using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common.Client;

#endregion

namespace BHIC.Core.Policy
{
    public class QuoteStatusService : IQuoteStatusService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns QuoteStatus based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public QuoteStatus GetQuoteStatus(QuoteStatusRequestParms args, ServiceProvider provider)
        {
            var quoteStatusResponse = SvcClient.CallService<QuoteStatusResponse>(string.Concat(Constants.ServiceNames.QuoteStatus, UtilityFunctions.CreateQueryString<QuoteStatusRequestParms>(args)), provider);

            if (quoteStatusResponse.OperationStatus.RequestSuccessful)
            {
                return quoteStatusResponse.QuoteStatus;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new QuoteStatus details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddQuoteStatus(QuoteStatus args, ServiceProvider provider)
        {
            var quoteStatusResponse = SvcClient.CallService<QuoteStatus, OperationStatus>(Constants.ServiceNames.QuoteStatus, "POST", args, provider);

            if (quoteStatusResponse.RequestSuccessful)
            {
                return quoteStatusResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.Messages));
            }

        }

        /// <summary>
        /// Delete existing QuoteStatus details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteQuoteStatus(QuoteStatusRequestParms args, ServiceProvider provider)
        {
            var quoteStatusResponse = SvcClient.CallService<QuoteStatusRequestParms, OperationStatus>(Constants.ServiceNames.QuoteStatus, "DELETE", args, provider);

            if (quoteStatusResponse.RequestSuccessful)
            {
                return quoteStatusResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quoteStatusResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
