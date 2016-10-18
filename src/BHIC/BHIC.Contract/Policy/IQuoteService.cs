#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common.Client;
using BHIC.Domain.PolicyCentre;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IQuoteService
    {
        /// <summary>
        /// Get the Quote specified by the associated request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Quote GetQuote(QuoteRequestParms args);


        List<QuoteResponse> GetQuoteList(List<string> quoteNumber);

        List<UserQuote> ViewSavedQuoteList(QuoteRequestParms args, List<UserQuote> lstUserQuote);

        List<UserQuote> ViewSavedBOPQuoteList(PCQuoteInformationRequestParms args, List<UserQuote> lstUserQuote);
    }
}
