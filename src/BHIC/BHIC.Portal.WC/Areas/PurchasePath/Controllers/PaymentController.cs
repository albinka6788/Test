#region Using directives

using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Common.Payment;
using BHIC.Common.Quote;
using BHIC.Common.Reattempt;
using BHIC.Common.XmlHelper;
using BHIC.Portal.WC.App_Start;
using System;
using System.Web.Mvc;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    public class PaymentController : BaseController
    {
        #region Variables

        public string CommonRedirectUrl
        {
            get
            {
                string paymentURLIP = ConfigCommonKeyReader.PaymentURLIP;

                loggingService.Trace(string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentURLIP) ? System.Web.HttpContext.Current.Request.Url.Host : paymentURLIP),
                    ConfigCommonKeyReader.PaymentURL + "PurchasePath/Payment/ProcessResponse"));
                return string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentURLIP) ? System.Web.HttpContext.Current.Request.Url.Host : paymentURLIP),
                    ConfigCommonKeyReader.PaymentURL + "PurchasePath/Payment/ProcessResponse");
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        //
        // GET: /PurchasePath/Payment/

        public ActionResult Index()
        {
            return View();
        }

        public string PaymentFooter { get { return "<div style='text-align: center;'>&copy; {0}. All Rights Reserved.</div>"; } }

        /// <summary>
        /// Generate dynamic payment form
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPaymentForm()
        {
            //fetch value from session, if exists already
            if (!IsCustomSessionNull())
            {
                BHIC.ViewDomain.CustomSession customSession = GetCustomSession();
                IPaymentProcess paymentProcess = new PaymentProcess();

                return Content(paymentProcess.ProcessPayment(new PaymentRequest
                {
                    AgencyCode = customSession.QuestionnaireVM.Agency,
                    PaymentAmount = customSession.QuoteSummaryVM.PaymentTerms.DownPayment,
                    FeeAmount = customSession.QuestionnaireVM.InstallmentFee,
                    //ContactName = customSession.PurchaseVM.PersonalContact.FirstName + " " + customSession.PurchaseVM.PersonalContact.LastName,
                    //ContactEmail = customSession.PurchaseVM.PersonalContact.Email,
                    //ContactPhone = customSession.PurchaseVM.PersonalContact.PhoneNumber,
                    FromPolicyCenter = false,
                    PaymentCode = Convert.ToString(QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext)),
                    IsPaymentRequestReceivedFromPurchasePath = true
                }, true).ReturnString);
            }
            else
            {
                throw new ApplicationException(string.Format("{0}: {1} !", Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
            }
        }

        public ActionResult ProcessResponse(string x_response_code, string x_response_reason_code, string x_response_reason_text, string x_auth_code, string x_avs_code, string x_trans_id, string x_invoice_num, string x_amount, string x_type, string x_MD5_Hash, string x_cvv2_resp_code, string x_cavv_response, string ds_apply_payment, string ds_customer_ip, string ds_contact_name, string ds_contact_email, string ds_contact_phone, string ds_agency_code, string ds_payment_code_type, string ds_confirm_type, string ds_submit_text, string ds_payment_amount, string ds_fee_amount)
        {

            //this section is only for testing payment gateway in dev environment
            if (ConfigCommonKeyReader.IsDevEnvironment)
            {
                Random rnd = new Random();
                BHIC.ViewDomain.CustomSession customSession = GetCustomSession();
                x_invoice_num = Convert.ToString(QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext));
                x_amount = customSession.QuoteSummaryVM.PaymentTerms.DownPayment.ToString();
                x_trans_id = Convert.ToString(rnd.Next(1, 100000));
                ds_payment_code_type = "true";
                x_response_code = "1";
            }

            IPaymentProcess paymentProcess = new PaymentProcess();

            var invoiceEncode = Server.UrlEncode(Encryption.EncryptText(string.Format("{0}", x_invoice_num)));

            //write payment reponse into log
            paymentProcess.WriteLogForPaymentResponse(x_response_code, x_response_reason_code, x_response_reason_text, x_auth_code, x_avs_code, x_trans_id, x_invoice_num, x_amount, x_type, x_MD5_Hash, x_cvv2_resp_code, x_cavv_response, ds_apply_payment, ds_customer_ip, ds_contact_name, ds_contact_email, ds_contact_phone, ds_agency_code, ds_payment_code_type, ds_confirm_type, ds_submit_text, ds_payment_amount, ds_fee_amount);

            if (string.IsNullOrEmpty(x_response_code) || !x_response_code.Equals("1"))
            {
                if (ConfigCommonKeyReader.IsTransactionLog)
                {
                    TransactionLogCustomSessions.CustomSessionForPaymentError(x_response_reason_text);
                }
                loggingService.Fatal(string.Format("Payment Failed with reason code {0} and text {1}", x_response_code, x_response_reason_text));

                var errorEncode = Server.UrlEncode(Encryption.EncryptText(string.Format("{0}", x_response_reason_text)));

                //set payment response false, on payment failure
                Session["PaymentResponse"] = (PaymentResponseViewModel)paymentProcess.SetPaymentResponseInSession(0, null, x_invoice_num, errorEncode, Convert.ToBoolean(ds_payment_code_type));

                return RedirectToAction("Index", "OrderSummary", new { area = "PurchasePath" });
            }

            loggingService.Trace("Payment Successful, navigating to order summary page");

            //set payment response true, after successful payment
            Session["PaymentResponse"] = (PaymentResponseViewModel)paymentProcess.SetPaymentResponseInSession(2, null, x_invoice_num, invoiceEncode, Convert.ToBoolean(ds_payment_code_type), x_trans_id, x_amount);

            return RedirectToAction("Index", "OrderSummary", new { area = "PurchasePath" });
        }

        #endregion

        #endregion
    }
}
