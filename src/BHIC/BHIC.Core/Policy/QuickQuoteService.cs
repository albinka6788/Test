using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common;

namespace BHIC.Core.Policy
{
    public class QuickQuoteService : IServiceProviders, IQuickQuoteService
    {
        #region Comment : Here constructor

        public QuickQuoteService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Return Premium Amt based on available-carrier and exposure-data using QuickQuote
        /// </summary>
        /// <returns></returns>
        public decimal GetQuickQuotePremium(QuickQuoteRequestParms args)
        {
            var quickQuoteResponse = SvcClient.CallServicePost<QuickQuoteRequestParms, QuickQuoteResponse>(Constants.QuickQuote, args, ServiceProvider);

            if (quickQuoteResponse.OperationStatus.RequestSuccessful)
            {
                return quickQuoteResponse.Premium;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(quickQuoteResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
