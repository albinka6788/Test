using System;
using System.Linq;
using System.Web.Mvc;
using BHIC.Common.Payment;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Policy;
using BHIC.Common;
using BHIC.Common.XmlHelper;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.Config;
using System.Reflection;
using Document = BHIC.Domain.Document.Document;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class MakePaymentController : BaseController
    {
        #region Variables

        public string CommonRedirectUrl
        {
            get
            {
                string paymentUrlip = ConfigCommonKeyReader.PaymentURLIP;

                loggingService.Trace(string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentUrlip) ? System.Web.HttpContext.Current.Request.Url.Host : paymentUrlip),
                    ConfigCommonKeyReader.PaymentURL + "PC/MakePayment/ProcessResponse"));

                return string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentUrlip) ? System.Web.HttpContext.Current.Request.Url.Host : paymentUrlip),
                    ConfigCommonKeyReader.PaymentURL + "PC/MakePayment/ProcessResponse");
            }
        }

        #endregion

        //
        // GET: /PolicyCentre/MakePayment/

        public ActionResult MakePayment(string CYBKey)
        {
            if (!string.IsNullOrEmpty(CYBKey) && CYBKey.StartsWith(Constants.FailedPaymentCode))
            {
                ViewBag.ErrorMessage = Constants.PaymentFailure;
            }

            return PartialView("MakePayment");
        }

        /// <summary>
        /// It will fetch billing related details from api
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBillingDetails(string CYBKey)
        {
            try
            {
                string optionSelected;
                string policyCode;
                bool isPaymentFailure = false;

                if (CYBKey.Contains("|:|"))
                {
                    if (CYBKey.Split(new string[] { "|:|" }, StringSplitOptions.None)[0].Equals(Constants.FailedPaymentCode))
                    {
                        isPaymentFailure = true;
                    }
                }

                if (!string.IsNullOrEmpty(CYBKey) && isPaymentFailure)
                {
                    string key = CYBKey.Split(new string[] { "|:|" }, StringSplitOptions.None)[1];
                    string decryptedQueryString = Encryption.DecryptText(key.Substring(0, key.Length - 1));
                    var policyNumber = decryptedQueryString.Split('=')[1].Split('&')[0];
                    optionSelected = "1";
                    policyCode = policyNumber;
                }
                else
                {
                    var encKey = CYBKey.Substring(0, CYBKey.Length - 1);
                    optionSelected = CYBKey.Substring(CYBKey.Length - 1, 1);
                    policyCode = DecryptedCYBKey(encKey).PolicyCode;
                }

                if (!string.IsNullOrEmpty(policyCode) && (optionSelected == "1" || optionSelected == "2"))
                {
                    IBillingService billingService = new BillingService(guardServiceProvider);

                    var billingDetails = billingService.GetBillingDetails(new BillingRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });

                    foreach (Document t in billingDetails.Billing.BillingDetails.BillingSummary.BillingStatements)
                    {
                        t.EncryptedDocumentId = Server.UrlEncode(Encryption.EncryptText(Convert.ToString(t.DocumentId)));
                    }

                    return Json(new
                    {
                        isSuccess = true,
                        payments = billingDetails.Billing.BillingDetails.Payments.OrderBy(x => x.PaymentDate).ToList(),
                        futureBills = billingDetails.Billing.BillingDetails.FutureBills.OrderBy(x => x.BillDate).ToList(),
                        totalPaid = billingDetails.Billing.BillingDetails.BillingSummary.TotalPaid,
                        currentDue = billingDetails.Billing.BillingDetails.BillingSummary.CurrentDue,
                        totalRemaining = billingDetails.Billing.BillingDetails.BillingSummary.AccountBalance,
                        documents = billingDetails.Billing.BillingDetails.BillingSummary.BillingStatements,
                        policyCode,
                        payOptions = GetPayOptions(),
                        amountReceived = (optionSelected == "1") ? billingDetails.Billing.BillingDetails.BillingSummary.CurrentDue : billingDetails.Billing.BillingDetails.BillingSummary.AccountBalance,
                        lable = SetDefaultPayOption(GetPayOptions(), optionSelected).FirstOrDefault(x => x.IsEnabled).Value
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw;
            }
        }

        /// <summary>
        /// It will generate payment form dynamically
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPaymentForm(string CYBKey)
        {
            try
            {
                IPaymentProcess paymentProcess = new PaymentProcess();

                string optionSelected;
                string policyCode;
                bool isPaymentFailure = false;

                if (CYBKey.Contains("|:|"))
                {
                    if (CYBKey.Split(new string[] { "|:|" }, StringSplitOptions.None)[0].Equals(Constants.FailedPaymentCode))
                    {
                        isPaymentFailure = true;
                    }
                }

                if (!string.IsNullOrEmpty(CYBKey) && isPaymentFailure)
                {
                    string key = CYBKey.Split(new string[] { "|:|" }, StringSplitOptions.None)[1];
                    string decryptedQueryString = Encryption.DecryptText(key.Substring(0, key.Length - 1));

                    var policyNumber = decryptedQueryString.Split('=')[1].Split('&')[0];

                    optionSelected = "1";
                    policyCode = policyNumber;
                }
                else
                {
                    var encKey = CYBKey.Substring(0, CYBKey.Length - 1);
                    optionSelected = CYBKey.Substring(CYBKey.Length - 1, 1);
                    policyCode = DecryptedCYBKey(encKey).PolicyCode;
                }

                if (optionSelected == "1" || optionSelected == "2")
                {
                    BillingResponse bDetails = GetBillingResponse(policyCode);
                    var user = UserSession();

                    return Content(paymentProcess.ProcessPayment(new PaymentRequest
                    {
                        AgencyCode = string.Empty,
                        PaymentAmount = (optionSelected == "1") ? bDetails.Billing.BillingDetails.BillingSummary.CurrentDue : bDetails.Billing.BillingDetails.BillingSummary.AccountBalance,
                        FeeAmount = 0,
                        ContactName = user.FirstName + user.LastName,
                        ContactEmail = user.Email,
                        ContactPhone = Convert.ToString(user.PhoneNumber),
                        FromPolicyCenter = true,
                        PaymentCode = policyCode,
                        IsPaymentRequestReceivedFromPurchasePath = false
                    }, false).ReturnString);
                }
                return null;
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                throw;
            }
        }

        /// <summary>
        /// Update values,When user changes payment option
        /// </summary>
        /// <param name="policyCode"></param>
        /// <returns></returns>
        public BillingResponse GetBillingResponse(string policyCode)
        {
            IBillingService billingService = new BillingService(guardServiceProvider);
            return billingService.GetBillingDetails(new BillingRequestParms { PolicyCode = policyCode, SessionId = Session.SessionID, UserId = UserSession().Email });
        }

    }
}
