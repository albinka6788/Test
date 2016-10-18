using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Linq;

namespace BHIC.Core.PolicyCentre
{
    public class BillingSummaryService : IServiceProviders, IBillingSummaryService
    {
        public BillingSummaryService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        public BillingSummaryResponse GetBillingSummary(Domain.Policy.BillingSummaryRequestParms args)
        {
            BillingSummaryResponse billingSummaryResponse = SvcClient.CallService<BillingSummaryResponse>(string.Concat(Constants.BillingSummary,
                UtilityFunctions.CreateQueryString<BillingSummaryRequestParms>(args)), ServiceProvider);

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
