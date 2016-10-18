namespace BHIC.Common.Payment
{
    public static class PaymentConstants
    {
        public static string FormName = "paymentForm";
        public static string SubmitButtonText = "Pay Now";

        public enum PaymentConfirmationTypeEnum
        {
            ExistingPolicy,
            NewPolicy
        }
    }
}
