#region Using directives

using BHIC.Common;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Config;
using BHIC.Common.Quote;
using BHIC.Contract.PurchasePath;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Policy;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain.BuyPolicy;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]
    [ValidateSession]
    [CustomAntiForgeryToken]
    public class PolicyPurchaseController : BaseController
    {
        #region Variables

        private static BHIC.Common.Logging.ILoggingService logger = BHIC.Common.Logging.LoggingService.Instance;

        #endregion

        #region Methods

        #region Public Methods

        //
        // GET: /PurchasePath/Payment/
        public ActionResult Index()
        {
            return View();
        }

        //public PartialViewResult PaymentForm()
        //{
        //    return PartialView("_PaymentForm");
        //}

        /// <summary>
        /// Buy Policy partial view
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BuyPolicy(string quoteId)
        {
            if (!string.IsNullOrEmpty(quoteId) && quoteId.StartsWith(Constants.FailedPaymentCode))
            {
                ViewBag.PaymentError = Constants.PaymentFailure;
            }

            //return to app error if it is not valid page request
            IsValidPageRequest();

            return PartialView("_BuyPolicy", GetPaymentPlans());
        }

        /// <summary>
        /// Return status whether this page resuest is valid or un-authorized
        /// </summary>
        /// <returns></returns>
        private bool IsValidPageRequest()
        {
            if (!IsCustomSessionNull())
            {
                //var customSession = GetCustomSession();

                //Comment : Here check is this/current quote have associated purchase-veiw-model(user-info) object otherwise invalid buy-policy page request
                //if (customSession.PurchaseVM.IsNull() || customSession.QuestionnaireVM.IsNull() || customSession.QuoteSummaryVM.IsNull() ||
                //    (!customSession.QuoteSummaryVM.IsNull() && !customSession.QuoteSummaryVM.PaymentTerms.IsNull() && customSession.QuoteSummaryVM.PaymentTerms.PaymentPlanId <= 0))
                //{
                //    throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get buy-policy page !", "BuyPolicyPage", Constants.UnauthorizedRequest));
                //}

                if (GetCustomSession().PageFlag < 5)
                {
                    throw new ApplicationException(string.Format("{0},{1} : You are not authorized to get buy-policy page !", "BuyPolicyPage", Constants.UnauthorizedRequest));
                }
            }
            else
            {
                throw new ApplicationException(string.Format("{0},{1} : {2} !", "BuyPolicy", Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
            }

            return true;
        }

        /// <summary>
        /// Get all available plans
        /// </summary>
        /// <returns></returns>
        private BuyPolicyViewModel GetPaymentPlans()
        {
            BuyPolicyViewModel buyPolicyVM = new BuyPolicyViewModel();

            //if session does not exist, redirect user to session expire page
            if (!IsCustomSessionNull())
            {
                var customSession = GetCustomSession();

                //Comment : Progress bar navigation links handling.
                List<NavigationModel> links = new List<NavigationModel>();
                NavigationController nc = new NavigationController();
                links = nc.GetProgressBarLinks(customSession.PageFlag);

                //if required values does not exists in session , redirect user to session expire page.
                //Installment fee may not be applicable in some situations, that's why it is not considered.
                if (customSession.PurchaseVM.PersonalContact.IsNull() || string.IsNullOrEmpty(customSession.PurchaseVM.PersonalContact.Email)
                    || customSession.QuestionnaireVM.PremiumAmt <= 0 || customSession.QuoteSummaryVM.PaymentPlans.IsNull() || customSession.QuoteSummaryVM.PaymentTerms.IsNull())
                {
                    throw new ApplicationException(string.Format("{0},{1} : {2} !", "BuyPolicy", Constants.SessionExpiredPartial, Constants.AppCustomSessionExpired));
                }

                buyPolicyVM.UserEmailId = customSession.PurchaseVM.PersonalContact.Email;
                buyPolicyVM.InstallmentFee = customSession.QuestionnaireVM.InstallmentFee;
                buyPolicyVM.PremiumAmt = customSession.QuestionnaireVM.PremiumAmt;
                buyPolicyVM.PaymentOptions = customSession.QuoteSummaryVM.PaymentPlans;
                buyPolicyVM.ProductName = Constants.ProductName;
                buyPolicyVM.PaymentTerms = customSession.QuoteSummaryVM.PaymentTerms;
                buyPolicyVM.NavLinks = links;
            }

            return buyPolicyVM;
        }

        /// <summary>
        /// It will update payment related session data
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdatePaymentSessionDetails(string selectedPlanId)
        {
            //if current session is lost,redirect user to home page
            if (!IsCustomSessionNull())
            {
                var listOfErrors = new List<string>();

                if (string.IsNullOrEmpty(selectedPlanId))
                {
                    listOfErrors.Add(Constants.EmptyPaymentPlan);

                    return Json(new
                    {
                        resultMessages = listOfErrors,
                        isSuccess = false,
                    }, JsonRequestBehavior.AllowGet);
                }

                BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

                //get selected plan from session
                var selectedPaymentPlan = ((customSession.QuoteSummaryVM == null) || (customSession.QuoteSummaryVM.PaymentPlans == null)) ? null : customSession.QuoteSummaryVM.PaymentPlans.Where(x => x.PaymentPlanId == Convert.ToInt32(selectedPlanId)).FirstOrDefault();

                //get questionnaireVM from session
                var questionnaireVM = customSession.QuestionnaireVM;

                //if required session values does not exists, redirect user to home page
                if (selectedPaymentPlan != null && questionnaireVM != null && questionnaireVM.PremiumAmt > 0)
                {
                    int wcQuoteId = QuoteCookieHelper.Cookie_GetQuoteId(this.ControllerContext.HttpContext);

                    if (wcQuoteId > 0)
                    {
                        IQuoteSummary quoteSummary = new QuoteSummary(GetCustomSession(), guardServiceProvider);
                        PaymentTerms paymentTerms = new PaymentTerms();

                        paymentTerms.QuoteId = Convert.ToInt32(wcQuoteId);
                        paymentTerms.PaymentPlanId = selectedPaymentPlan.PaymentPlanId;
                        paymentTerms.DownPayment = quoteSummary.GetDownPaymentAmount(Convert.ToDecimal(questionnaireVM.PremiumAmt), selectedPaymentPlan);
                        paymentTerms.Installments = selectedPaymentPlan.Pays;
                        paymentTerms.InstallmentAmount = quoteSummary.GetFutureInstallmentAmount(Convert.ToDecimal(questionnaireVM.PremiumAmt), selectedPaymentPlan);
                        paymentTerms.InstallmentFee = Convert.ToDecimal(questionnaireVM.InstallmentFee);
                        paymentTerms.Frequency = "";
                        paymentTerms.FrequencyCode = selectedPaymentPlan.Freq;

                        //Comment : Here update this payment plan into service provider system
                        var paymentTermsResponse = quoteSummary.AddPaymentTerms(paymentTerms);

                        if (paymentTermsResponse != null && !paymentTermsResponse.RequestSuccessful)
                        {
                            paymentTermsResponse.Messages.ForEach(msg => listOfErrors.Add(msg.Text));

                            //Comment : Here retrun execution of code from here itself in case of some error occured
                            if (listOfErrors.Count > 0)
                            {
                                //return json data
                                return Json(new
                                {
                                    isSuccess = false,
                                    resultMessages = listOfErrors
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if (paymentTermsResponse.RequestSuccessful)
                        {
                            //when user change payment plan, upadate quote data 
                            var quoteCreatedinDB = false;

                            try
                            {
                                //Comment : Here get logged PC user id
                                var loggedUserId = GetLoggedUserId();

                                quoteCreatedinDB = quoteSummary.AddUpdateQuoteData(new QuoteDTO
                                {
                                    QuoteNumber = wcQuoteId.ToString(),
                                    PremiumAmount = Convert.ToDecimal(questionnaireVM.PremiumAmt),
                                    LineOfBusinessId = 1, //ToDo: Replace hard coded value with actual values
                                    ExternalSystemID = 1, //ToDo: Replace hard coded value with actual values
                                    IsActive = true,
                                    RequestDate = DateTime.Now,
                                    PaymentOptionID = selectedPaymentPlan.PaymentPlanId,
                                    AgencyCode = customSession.QuestionnaireVM.Agency,
                                    CreatedBy = loggedUserId ?? 1,
                                    CreatedDate = DateTime.Now,
                                    ExpiryDate = DateTime.Now.AddYears(1),
                                    ModifiedDate = DateTime.Now,
                                    ModifiedBy = loggedUserId ?? 1
                                });
                            }
                            catch (Exception ex)
                            {
                                loggingService.Fatal(ex);
                            }

                            //update session with new values
                            customSession.QuoteSummaryVM.PaymentTerms = paymentTerms;
                            customSession.QuoteSummaryVM.SelectedPaymentPlan = selectedPaymentPlan;
                        }
                    }

                    SetCustomSession(customSession);

                    //return json data
                    return Json(new
                    {
                        isSuccess = true,
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            //return json data
            return Json(new
            {
                isSessionExpired = true,
                isSuccess = false,
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion
    }
}
