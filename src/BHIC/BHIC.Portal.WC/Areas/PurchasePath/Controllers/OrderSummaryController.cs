#region Using directives

using System;
using System.Web.Mvc;
using System.Linq;

using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Common.Quote;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Policy;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.Domain.Policy;
using BHIC.ViewDomain;
using BHIC.Portal.WC.App_Start;
using BHIC.ViewDomain.Mailing;
using System.Collections.Generic;
using BHIC.Common.Configuration;
using BHIC.ViewDomain.QuestionEngine;
using BHIC.DML.WC.DTO;
using BHIC.Contract.Mailing;
using BHIC.Core.Mailing;
using BHIC.ViewDomain.OrderSummary;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Common.CommonUtilities;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.PolicyCentre;

#endregion

namespace BHIC.Portal.WC.Areas.PurchasePath.Controllers
{
    //[CustomTransactionLogFilterAttribute]        
    public class OrderSummaryController : BaseController
    {
        #region Methods

        #region Public Methods

        //
        // GET: /PurchasePath/OrderSummary/

        public ActionResult Index()
        {
            loggingService.Trace("Order summary loaded");

            return View();
        }

        //public ActionResult Confirmation(string transactionCode)
        //{
        //    if (!IsCustomSessionNull() && !string.IsNullOrEmpty(transactionCode))
        //    {
        //        BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

        //        loggingService.Trace(string.Format("Confirmaton Transaction Code {0}", (string.IsNullOrEmpty(transactionCode) ? "Empty Transaction Code" : transactionCode)));
        //        customSession.TransactionCode = transactionCode;

        //        SetCustomSession(customSession);
        //        return View();
        //    }

        //    return RedirectToAction("SessionExpired", "Error");
        //}

        //public PartialViewResult ConfirmationContent()
        //{
        //    if (!IsCustomSessionNull())
        //    {
        //        BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

        //        loggingService.Trace(string.Format("Confirmation Content Transaction Code {0}",
        //                (string.IsNullOrEmpty(customSession.TransactionCode) ? "Empty Transaction Code" : customSession.TransactionCode)));

        //        //decrypt query string parameters
        //        string decryptedQueryString = Encryption.DecryptText(customSession.TransactionCode);

        //        var invoiceNumber = decryptedQueryString.Split('=')[1].Split('&')[0];
        //        var transactionId = decryptedQueryString.Split('=')[2].Split('&')[0];
        //        var amount = decryptedQueryString.Split('=')[3];

        //        //Store policy details into database
        //        SavePolicyDetails(invoiceNumber, transactionId, amount);

        //        return PartialView("_ConfirmationContent");
        //    }

        //    return PartialView("_SessionExpiredPayment");
        //}

        [ValidateSession]
        [CustomAntiForgeryToken]
        public PartialViewResult ConfirmationContent(string transactionCode)
        {
            if (!IsCustomSessionNull() && !string.IsNullOrEmpty(transactionCode))
            {
                //Comment : Here To pass QuoteId requirement first capture running QuoteId from session 
                ViewBag.QuoteId = ((BHIC.ViewDomain.CustomSession)Session["CustomSession"]).QuoteVM.QuoteId;

                //Comment : Here initialize new app session 
                BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

                loggingService.Trace(string.Format("Confirmaton Transaction Code {0}", (string.IsNullOrEmpty(transactionCode) ? "Empty Transaction Code" : transactionCode)));
                customSession.TransactionCode = transactionCode;

                SetCustomSession(customSession);
                return PartialView("_ConfirmationContent");
            }

            return PartialView("_SessionExpiredPayment");
        }

        [ValidateSession]
        [CustomAntiForgeryToken]
        public JsonResult GetOrderSummary()
        {
            var listOfErrors = new List<string>();

            if (!IsCustomSessionNull())
            {
                BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

                loggingService.Trace(string.Format("Confirmation Content Transaction Code {0}",
                        (string.IsNullOrEmpty(customSession.TransactionCode) ? "Empty Transaction Code" : customSession.TransactionCode)));

                if (!string.IsNullOrEmpty(customSession.TransactionCode))
                {
                    //decrypt query string parameters
                    string decryptedQueryString = Encryption.DecryptText(customSession.TransactionCode);

                    var invoiceNumber = decryptedQueryString.Split('=')[1].Split('&')[0];
                    var transactionId = decryptedQueryString.Split('=')[2].Split('&')[0];
                    var amount = decryptedQueryString.Split('=')[3];

                    //return json data
                    return Json(new
                    {
                        isSuccess = true,
                        data = SavePolicyDetails(invoiceNumber, transactionId, amount),
                    }, JsonRequestBehavior.AllowGet);
                }

                listOfErrors.Add("Transaction code is invalid");

                //return json data filled with list of errors
                return Json(new
                {
                    isSuccess = false,
                    resultMessages = listOfErrors
                }, JsonRequestBehavior.AllowGet);
            }

            //return json data
            return Json(new
            {
                isSuccess = false,
                isSessionExpired = true
            }, JsonRequestBehavior.AllowGet);


        }

        #endregion

        #region Private Methods

        /// <summary>
        /// It will store policy details into database
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="transactionId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private OrderSummaryViewModel SavePolicyDetails(string invoiceNumber, string transactionId, string amount)
        {
            //Session["PaymentResponse"] = new BHIC.ViewDomain.CustomSession();
            OrderSummaryViewModel orderSummary = new OrderSummaryViewModel();

            BHIC.ViewDomain.CustomSession customSession = GetCustomSession();

            //Comment : Here get logged PC user id
            var loggedUserId = GetLoggedUserId();

            //Get quote id from cookie
            int wcQuoteId = Convert.ToInt32(invoiceNumber);

            if (wcQuoteId > 0)
            {
                try
                {
                    #region Comment : Here Must check user account registration status based on EmailId and IsActive status

                    loggingService.Trace(string.Format("Check whether user is existing or new & fetch payment term start at {0}", DateTime.Now));

                    var eligibleForRegistrationMail = false;

                    //Comment : Here create BLL instance
                    PurchaseQuote purchaseQuoteBLL = new PurchaseQuote();

                    //Comment : Here get user credentials details
                    BHIC.DML.WC.DTO.OrganisationUserDetailDTO organisationUser = new DML.WC.DTO.OrganisationUserDetailDTO();
                    organisationUser.EmailID = customSession.PurchaseVM.Account.Email.Trim();

                    //Comment : Here get user data 

                    BHIC.DML.WC.DTO.OrganisationUserDetailDTO orgUserCredential = purchaseQuoteBLL.GetUserCredentialDetails(organisationUser);

                    //Comment : Here if data found
                    eligibleForRegistrationMail = (orgUserCredential != null && organisationUser.EmailID.Length > 0 && !orgUserCredential.IsActive) ? true : false;

                    #endregion

                    #region Comment : Here STEP - 1. On succesful payment processing a)Add payment detail in DB b)Activate user to allow login in PC

                    IQuoteDataProvider quoteDataProvider = new QuoteDataProvider();
                    PaymentTerms paymentTerms = null;

                    if (customSession != null && customSession.QuoteSummaryVM != null && customSession.QuoteSummaryVM.PaymentTerms != null &&
                        customSession.QuoteSummaryVM.PaymentTerms.PaymentPlanId > 0)
                    {
                        paymentTerms = customSession.QuoteSummaryVM.PaymentTerms;
                    }
                    else
                    {
                        IPaymentTermService paymentTermsService = new PaymentTermService(guardServiceProvider);
                        paymentTerms = paymentTermsService.GetPaymentTermsList(new PaymentTermsRequestParms { QuoteId = wcQuoteId });
                    }

                    loggingService.Trace(string.Format("Check whether user is existing or new & fetch payment term stop at {0}", DateTime.Now));

                    string mgaCode = string.Empty;
                    //IPaymentService paymentService = new PaymentService(guardServiceProvider);
                    IGeneratePolicy generatePolicy = new GeneratePolicy();

                    try
                    {
                        loggingService.Trace(string.Format("Policy creation start at {0}", DateTime.Now));

                        //Create policy and generate mgaCode
                        mgaCode = generatePolicy.CreatePolicy(Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC), customSession.PurchaseVM.Account.Email, wcQuoteId, guardServiceProvider);

                        loggingService.Trace(string.Format("Policy creation stop at {0}", DateTime.Now));

                        //Set values into session
                        // SetPaymentIntoSession(wcQuoteId, mgaCode, transactionId);

                        orderSummary = SetPolicyDetails(orderSummary, wcQuoteId, mgaCode, transactionId);

                        // SetPaymentResponseInSession();

                        orderSummary = SetPolicyReponseStatus(orderSummary);
                    }
                    catch (Exception ex)
                    {
                        //set payment response false, on payment failure
                        orderSummary = SetPolicyReponseStatus(orderSummary, 1, ex.Message);

                        // SetPaymentResponseInSession(1, ex.Message);

                        loggingService.Fatal(ex);

                        try
                        {
                            IMailingService mailingService = new MailingService();
                            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                            #region Comment : Here if policy creation failed, send payment related details through mail

                            mailingService.PaymentFailure(new PaymentFailureViewModel
                            {
                                QuoteIdHeader = "Quote ID",
                                QuoteId = Convert.ToString(wcQuoteId),
                                LOBHeader = "LOB",
                                LOB = Constants.GetLineOfBusiness(Constants.LineOfBusiness.WC),
                                PaymentConfirmationNoHeader = "Payment Confirmation no",
                                PaymentConfirmationNo = transactionId,
                                TransactionAmountHeader = "Transaction Amount",
                                TransactionAmount = amount,
                                FailureMessageHeader = "Failure Message",
                                FailureMessage = ex.Message,
                                FailureDateTimeHeader = "Failure Date/Time",
                                FailureDateTime = DateTime.Now.ToString(),
                                YourIPAddressHeader = "Your IP Address",
                                YourIPAddress = "",
                                YourEmailAddressHeader = "Your Email Address",
                                YourEmailAddress = customSession.PurchaseVM.Account.Email,
                                YourPhoneNumberHeader = "Your Phone Number",
                                YourPhoneNumber = customSession.PurchaseVM.PersonalContact.PhoneNumber,
                                YourNameHeader = "Your Name",
                                YourName = string.Concat(customSession.PurchaseVM.BusinessContact.FirstName, " ", customSession.PurchaseVM.BusinessContact.LastName),
                                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                                SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])),
                                SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]),
                                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"],
                                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"],
                                YourEmailAddressHref = string.Format("mailto:{0}", customSession.PurchaseVM.Account.Email),
                                AbsoulteURL = CDN.GetEmailImageUrl(),
                            });

                            #endregion
                        }
                        catch (Exception exMail)
                        {
                            loggingService.Fatal(exMail);
                        }
                    }

                    loggingService.Trace(string.Format("Database activities for created policy start at {0}", DateTime.Now));

                    BHIC.Contract.PurchasePath.IPaymentDataService payment = new BHIC.Core.PurchasePath.PaymentDataService();

                    //Store policy data into database
                    int policyId = payment.AddPolicy(new PolicyDTO
                      {
                          QuoteNumber = wcQuoteId.ToString(),
                          PolicyNumber = mgaCode,
                          EffectiveDate = DateTime.Now,
                          ExpiryDate = DateTime.Now,
                          PremiumAmount = Convert.ToDecimal(amount),
                          PaymentOptionID = Convert.ToInt32(paymentTerms.PaymentPlanId),
                          IsActive = !string.IsNullOrEmpty(mgaCode),
                          CreatedDate = DateTime.Now,
                          CreatedBy = loggedUserId ?? 1,
                          ModifiedDate = DateTime.Now,
                          ModifiedBy = loggedUserId ?? 1
                      },
                      new PolicyPaymentDetailDTO
                      {
                          TransactionCode = transactionId,
                          AmountPaid = Convert.ToDecimal(amount),
                          IsActive = true,
                          CreatedDate = DateTime.Now,
                          CreatedBy = loggedUserId ?? 1,
                          ModifiedDate = DateTime.Now,
                          ModifiedBy = loggedUserId ?? 1
                      });

                    loggingService.Trace(string.Format("Database activities for created policy stop at {0}", DateTime.Now));

                    #endregion

                    if (customSession != null && mgaCode.Length > 0 && policyId > 0)
                    {
                        #region Comment : Here STEP - 2. After successful payment details tracking in DB send user Welcome/Registration mail

                        try
                        {
                            #region Comment : Here STEP - 2(a). Send Welcome mail first

                            loggingService.Trace(string.Format("Sending Welcome mail start at {0}", DateTime.Now));

                            #region Comment : Here prepare VM for templatize mail body

                            //Comment : Here get purchase view-model
                            var sessionPurchaseVM = customSession.PurchaseVM ?? new WcPurchaseViewModel();
                            //inception date
                            var inceptionDate = customSession.QuoteVM.PolicyData.InceptionDate.Value;
                            //next due date
                            var selectedPaymentTerm = customSession.QuoteSummaryVM.PaymentTerms;
                            // var nextDueDate = CalculatePolicyNextDueDate(inceptionDate, selectedPaymentTerm);

                            var nextDueDate = GetBillingResponse(mgaCode, customSession.PurchaseVM.Account.Email.Trim());

                            //get next instalment due amount
                            var nextInstallmentAmount = selectedPaymentTerm != null ? Math.Round(Convert.ToDecimal(selectedPaymentTerm.InstallmentAmount), 2).ToString("#,##0.00") : string.Empty;


                            //Comment : Here set VM values
                            var model = new PolicyWelcomeMailViewModel
                            {
                                //Basic policy related details
                                InsuredBusinessName =
                                (
                                    (!sessionPurchaseVM.IsNull() && !sessionPurchaseVM.BusinessInfo.IsNull() && sessionPurchaseVM.BusinessInfo.BusinessType != "I" && !string.IsNullOrEmpty(sessionPurchaseVM.BusinessInfo.BusinessName)) ?
                                    sessionPurchaseVM.BusinessInfo.BusinessName :
                                    (
                                        (!sessionPurchaseVM.IsNull() && !sessionPurchaseVM.BusinessInfo.IsNull() && !string.IsNullOrEmpty(sessionPurchaseVM.BusinessInfo.FirstName)) ?
                                        string.Concat(sessionPurchaseVM.BusinessInfo.FirstName, " ", sessionPurchaseVM.BusinessInfo.LastName) :
                                        string.Empty
                                    )
                                ),
                                PolicyNumber = mgaCode,
                                PolicyEffectiveDate = inceptionDate,
                                PolicyEffectiveDateString = customSession.QuoteVM.PolicyData.InceptionDate.Value.ToString("MM/dd/yyyy"),

                                //Policy billing related details
                                TotalPremiumAmount = customSession.QuestionnaireVM.PremiumAmt > 0 ? customSession.QuestionnaireVM.PremiumAmt.ToString("#,##0.00") : string.Empty,
                                PremiumAmountPaid = !string.IsNullOrEmpty(amount) ? Convert.ToDecimal(amount).ToString("#,##0.00") : string.Empty,
                                PolicyInstalments = customSession.QuoteSummaryVM.PaymentTerms.Installments,
                                PolicyNextInstallmentAmount = customSession.QuoteSummaryVM.PaymentTerms.InstallmentAmount.Value,
                                PolicyNextDueDate = (nextDueDate == null) ? DateTime.MinValue : (DateTime)nextDueDate,
                                PolicyNextDueDateString = nextDueDate != null ? ((DateTime)nextDueDate).ToString("MM/dd/yyyy") : "none",
                                NextInstallmentAmount = nextInstallmentAmount
                            };

                            #endregion

                            SendMailPolicyWelcome(string.IsNullOrEmpty(customSession.UserEmailId) ? customSession.PurchaseVM.Account.Email : customSession.UserEmailId, model);

                            loggingService.Trace(string.Format("Sending Welcome mail stop at {0}", DateTime.Now));

                            #endregion

                            #region Comment : Here STEP - 2(b). Send Registration mail if user policy creation Welcome mail sent sucessfully

                            //Comment : Here in case welcome mail sent then do following 
                            if (eligibleForRegistrationMail)
                            {
                                loggingService.Trace(string.Format("Sending Registration mail start at {0}", DateTime.Now));

                                SendMailUserAccountRegistration(customSession.PurchaseVM.Account.Email,
                                    new PolicyRegistrationMailViewModel
                                    {
                                        UserName = string.Concat(customSession.PurchaseVM.PersonalContact.FirstName, " ", customSession.PurchaseVM.PersonalContact.LastName)
                                    });

                                loggingService.Trace(string.Format("Sending Registration mail stop at {0}", DateTime.Now));
                            }

                            #endregion
                        }
                        catch (Exception ex) { loggingService.Fatal(ex, "Error occured while processing Welcome/Registration mail !"); }

                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    loggingService.Fatal(ex);
                }
            }
            else
            {
                // SetPaymentResponseInSession(1);
                orderSummary = SetPolicyReponseStatus(orderSummary, 1);
            }

            // Added by Guru for API Log, please don't remove it
            Session["APILog"] = null;
            SetCustomSession(null);

            return orderSummary;
        }

        /// <summary>
        /// Save payment information in Session
        /// </summary>
        /// <param name="wcQuoteId"></param>
        /// <param name="mgaCode"></param>
        /// <param name="paymentConfirmation"></param>
        private void SetPaymentIntoSession(int wcQuoteId, string mgaCode, string paymentConfirmation)
        {
            BHIC.ViewDomain.CustomSession customSession = (BHIC.ViewDomain.CustomSession)Session["PaymentResponse"];

            if (wcQuoteId > 0)
            {
                customSession.QuoteSummaryVM = customSession.QuoteSummaryVM == null ? new QuoteSummaryViewModel() : customSession.QuoteSummaryVM;

                // Save QuoteId into session
                customSession.QuoteSummaryVM.QuoteReferenceNo = Convert.ToString(wcQuoteId);
            }

            if (!string.IsNullOrEmpty(mgaCode))
            {
                //initialize QuoteViewModel into session
                customSession.QuoteVM = customSession.QuoteVM == null ? new ViewDomain.Landing.QuoteViewModel() : customSession.QuoteVM;

                //initialize PolicyData
                customSession.QuoteVM.PolicyData = customSession.QuoteVM.PolicyData == null ? new PolicyData() : customSession.QuoteVM.PolicyData;

                customSession.QuoteVM.PolicyData.MgaCode = mgaCode;
            }

            if (!string.IsNullOrEmpty(paymentConfirmation))
            {
                customSession.PaymentConfirmationNumber = paymentConfirmation;
            }

            Session["PaymentResponse"] = customSession;
        }

        /// <summary>
        /// It will set payment response into session
        /// </summary>
        /// <param name="isSuccess"></param>
        private void SetPaymentResponseInSession(int isSuccess = 2, string paymentErrorMessage = "")
        {
            BHIC.ViewDomain.CustomSession customSession = (BHIC.ViewDomain.CustomSession)Session["PaymentResponse"];

            customSession.PaymentErrorMessage = paymentErrorMessage;
            customSession.IsPaymentSuccess = isSuccess;
            customSession.PolicyCentreURL = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.PolicyCentreDashboardURL);

            Session["PaymentResponse"] = customSession;
        }

        /// <summary>
        /// It will assign order summary into into OrderSummaryViewModel
        /// </summary>
        /// <param name="orderSummary"></param>
        /// <param name="wcQuoteId"></param>
        /// <param name="mgaCode"></param>
        /// <param name="paymentConfirmation"></param>
        /// <returns></returns>
        private OrderSummaryViewModel SetPolicyDetails(OrderSummaryViewModel orderSummary, int wcQuoteId, string mgaCode, string paymentConfirmation)
        {
            if (wcQuoteId > 0)
            {
                orderSummary.WcQuoteId = Convert.ToString(wcQuoteId);
            }

            if (!string.IsNullOrEmpty(mgaCode))
            {
                orderSummary.MgaCode = mgaCode;
            }

            if (!string.IsNullOrEmpty(paymentConfirmation))
            {
                orderSummary.PaymentConfirmationNumber = paymentConfirmation;
            }

            return orderSummary;
        }

        /// <summary>
        /// It will save payment realted error info into OrderSummaryViewModel
        /// </summary>
        /// <param name="orderSummary"></param>
        /// <param name="isSuccess"></param>
        /// <param name="paymentErrorMessage"></param>
        /// <returns></returns>
        private OrderSummaryViewModel SetPolicyReponseStatus(OrderSummaryViewModel orderSummary, int isSuccess = 2, string paymentErrorMessage = "")
        {
            orderSummary.PaymentErrorMessage = paymentErrorMessage;
            orderSummary.IsPaymentSuccess = isSuccess;
            orderSummary.PolicyCentreURL = string.Concat(GetSchemeAndHostURLPart(), ConfigCommonKeyReader.PolicyCentreDashboardURL);

            // Moved common setting here as it required in all cases i.e. Policy creation successful and Policy creation failed.
            orderSummary.ProductName = Constants.WC;

            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            orderSummary.PhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]);
            orderSummary.CompanyDomain = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);

            return orderSummary;
        }

        /// <summary>
        /// Method will send mail to user who has brought new policy
        /// </summary>
        /// <param name="pageMethod"></param>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        private bool SendMailPolicyWelcome(string emailId, PolicyWelcomeMailViewModel policyWelcomeMailVM)
        {
            try
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                policyWelcomeMailVM.AbsoulteURL = CDN.GetEmailImageUrl();
                policyWelcomeMailVM.WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);
                policyWelcomeMailVM.WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]);
                policyWelcomeMailVM.SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextWelcome"];
                policyWelcomeMailVM.SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefWelcome"];
                policyWelcomeMailVM.SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]);
                policyWelcomeMailVM.SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
                policyWelcomeMailVM.Physical_Address2 = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_Address2"]);
                policyWelcomeMailVM.Physical_AddressCSZ = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Physical_AddressCSZ"]);

                policyWelcomeMailVM.TargetUrl = string.Format("{0}{1}#{2}", GetSchemeAndHostURLPart(), ConfigCommonKeyReader.PolicyCentreURL, "Login");

                IMailingService mailingService = new MailingService();
                return mailingService.WelcomeMail(emailId, policyWelcomeMailVM);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(ex);
            }
            return false;
        }

        private bool SendMailUserAccountRegistration(string emailId, PolicyRegistrationMailViewModel policyRegistrationMailVM)
        {
            try
            {
                ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
                ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

                var encryptedURL = Encryption.EncryptText(emailId);

                policyRegistrationMailVM.CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"];
                policyRegistrationMailVM.AbsoulteURL = CDN.GetEmailImageUrl();
                policyRegistrationMailVM.WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]);
                policyRegistrationMailVM.WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]);
                policyRegistrationMailVM.RegisterEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextRegistration"];
                policyRegistrationMailVM.RegisterEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefRegistration"];
                policyRegistrationMailVM.SupportPhoneNumber = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]);
                policyRegistrationMailVM.SupportPhoneNumberHref = string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));

                policyRegistrationMailVM.TargetUrl = string.Format("{0}{1}#{2}/{3}", GetSchemeAndHostURLPart(), ConfigCommonKeyReader.PolicyCentreURL, "Login", Server.UrlEncode(encryptedURL));

                IMailingService mailingService = new MailingService();
                return mailingService.UserRegistrationMail(emailId, policyRegistrationMailVM);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(ex);
            }
            return false;
        }

        /// <summary>
        /// Calculate next due date based on slected payment term by user
        /// </summary>
        /// <param name="selectedPaymentTerms"></param>
        /// <returns></returns>
        private DateTime? CalculatePolicyNextDueDate(DateTime? inceptionDate, PaymentTerms selectedPaymentTerms)
        {
            //return value
            DateTime? nextDueDate = null;

            //Comment : Here set next payment details
            if (inceptionDate != null && selectedPaymentTerms != null && selectedPaymentTerms.Installments > 0)
            {
                //Comment : Here based on ferquency code decide next date
                //E	Bi-Monthly Installment
                //A	Annual Billing
                //Q	Quarterly Installments
                //W	Weekly Installments
                //M	Monthly Installments
                //S	Semi-Annual Installment
                //B	Bi-Weekly Installments
                //N	Semi-Monthly Installments
                switch (selectedPaymentTerms.FrequencyCode)
                {
                    case "E":
                        break;
                    case "A":
                        nextDueDate = inceptionDate.Value.AddYears(1);
                        break;
                    case "Q":
                        nextDueDate = inceptionDate.Value.AddMonths(3);
                        break;
                    case "W":
                        nextDueDate = inceptionDate.Value.AddDays(7);
                        break;
                    case "M":
                        nextDueDate = inceptionDate.Value.AddMonths(1);
                        break;
                    case "S":
                        //e.g : {"PaymentPlanId":1318,"Fplan":"D","Description":"60% Down + Balance in 6 Months","MinPrem":500.0000,"DownType":"%","Down":60,"Pays":1,"Freq":"S","FplanExt":"D,60,1,S"},
                        nextDueDate = inceptionDate.Value.AddMonths(6);
                        break;
                    case "B":
                        break;
                    case "N":
                        break;
                }
            }

            return nextDueDate;
        }

        /// <summary>
        /// Update values,When user changes payment option
        /// </summary>
        /// <param name="policyCode"></param>
        /// <returns></returns>
        private DateTime? GetBillingResponse(string policyCode, string email)
        {
            IBillingService billingService = new BillingService(guardServiceProvider);

            //fetch billingDetails
            var billingResponse = billingService.GetBillingDetails(
                new BillingRequestParms
                {
                    PolicyCode = policyCode,
                    SessionId = Session.SessionID,
                    UserId = email
                });

            //fetch next due date from BillingResponse
            if (billingResponse.Billing != null && billingResponse.Billing.BillingDetails != null
                && billingResponse.Billing.BillingDetails.FutureBills != null && billingResponse.Billing.BillingDetails.FutureBills.Count > 0)
            {
                return billingResponse.Billing.BillingDetails.FutureBills.OrderBy(x => x.BillDate).FirstOrDefault().DueDate;
            }

            return null;
        }

        #endregion

        #endregion
    }
}
