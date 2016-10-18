#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common.Client;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IQuoteStatusService
    {
        /// <summary>
        /// Returns QuoteStatus based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        QuoteStatus GetQuoteStatus(QuoteStatusRequestParms args,ServiceProvider provider);

        /// <summary>
        /// Save new QuoteStatus details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddQuoteStatus(QuoteStatus args,ServiceProvider provider);

        /// <summary>
        /// Delete existing QuoteStatus details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteQuoteStatus(QuoteStatusRequestParms args, ServiceProvider provider);
    }
}
