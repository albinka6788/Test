#region Using directives

using System;

#endregion

namespace BHIC.Common.Payment
{
    public interface IPaymentProcess
    {
        #region Methods

        PaymentResponse ProcessPayment(PaymentRequest transaction, bool isCalledFromWC = true);
        PaymentResponseViewModel SetPaymentResponseInSession(int isSuccess, string paymentErrorMessage = "", string invoiceNumber = "", string encodedInvoiceNumber = "", bool isPaymentRequestReceivedFromPurchasePath = true, string transactionId = "", string amount = "");
        bool WriteLogForPaymentResponse(string x_response_code, string x_response_reason_code, string x_response_reason_text, string x_auth_code, string x_avs_code, string x_trans_id, string x_invoice_num, string x_amount, string x_type, string x_MD5_Hash, string x_cvv2_resp_code, string x_cavv_response, string ds_apply_payment, string ds_customer_ip, string ds_contact_name, string ds_contact_email, string ds_contact_phone, string ds_agency_code, string ds_payment_code_type, string ds_confirm_type, string ds_submit_text, string ds_payment_amount, string ds_fee_amount);

        #endregion
    }
}
