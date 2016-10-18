using BHIC.Contract.PurchasePath;
using BHIC.DML.WC;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;

namespace BHIC.Core.PurchasePath
{
    public class PaymentDataService : IPaymentDataService
    {
        #region Methods

        #region Interface Implementation

        int IPaymentDataService.AddPolicy(PolicyDTO policy, PolicyPaymentDetailDTO policyPaymentDetails = null)
        {
            IPolicyDataProvider policyDataProvider = new PolicyDataProvider();

            //Store policy data into database
            return policyDataProvider.AddPolicyData(policy, policyPaymentDetails);
        }

        void IPaymentDataService.AddPaymentDetails(PolicyPaymentDetailDTO policyPaymentDetails)
        {
            IPolicyPaymentDetailProvider paymentDetailProvider = new PolicyPaymentDetailProvider();

            //Store policy payment detail into database
            paymentDetailProvider.AddPolicyPaymentDetail(policyPaymentDetails);
        }
        #endregion

        #endregion
    }
}
