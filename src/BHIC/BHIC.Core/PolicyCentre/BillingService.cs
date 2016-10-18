#region Using directives

using System;
using System.Linq;

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;

#endregion

namespace BHIC.Core.PolicyCentre
{
    public class BillingService : IServiceProviders, IBillingService
    {
        public BillingService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        BillingResponse IBillingService.GetBillingDetails(Domain.Policy.BillingRequestParms args)
        {
            BillingResponse billingSummaryResponse = SvcClient.CallService<BillingResponse>(string.Concat(Constants.Billing,
                UtilityFunctions.CreateQueryString<BillingRequestParms>(args)), ServiceProvider);

            if (billingSummaryResponse.OperationStatus.RequestSuccessful ||
                (billingSummaryResponse.OperationStatus.RequestProcessed && billingSummaryResponse.OperationStatus.Messages.FirstOrDefault().MessageType != MessageType.SystemError))
            {
                return billingSummaryResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(billingSummaryResponse.OperationStatus.Messages));
            }
        }
    }
}
