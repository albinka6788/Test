#region Using directives

using BHIC.DML.WC;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IPaymentDataService
    {
        int AddPolicy(PolicyDTO policy, PolicyPaymentDetailDTO policyPaymentDetails = null);

        void AddPaymentDetails(PolicyPaymentDetailDTO policyPaymentDetails);
    }
}
