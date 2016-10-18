using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Policy;
using BHIC.Core.PolicyCentre;
using BHIC.Core.PurchasePath;
using BHIC.DML.WC;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DataService;
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Portal.Dashboard.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class PolicyInformationController : BaseController
    {
        private readonly IPolicyDetailsService _policyDetailsService;
        private readonly IBillingSummaryService _billingSummaryService;
        private readonly IDashboardService _dashboardService;
        private readonly IBusinessTypeService _businessTypeService;
        private readonly IQuoteService _quoteService;
        List<BusinessType> _businessTypeResponse;

        public PolicyInformationController()
        {

            _policyDetailsService = new PolicyDetailsService(guardServiceProvider);
            _billingSummaryService = new BillingSummaryService(guardServiceProvider);
            _dashboardService = new DashboardService();
            _businessTypeService = new BusinessTypeService();
            _quoteService = new QuoteService(guardServiceProvider);
        }

        public ActionResult PolicyInformation()
        {
            return PartialView("PolicyInformation");
        }

        private bool AddPaymentDetails(string transactionCode, ref string transactionID, PolicyDetailsResponse pdResponse)
        {
            //update payment info
            if (!string.IsNullOrEmpty(transactionCode))
            {
                try
                {
                    //decrypt query string parameters
                    string decryptedQueryString = Encryption.DecryptText(transactionCode);

                    var policyNumber = decryptedQueryString.Split('=')[1].Split('&')[0];
                    var transactionId = decryptedQueryString.Split('=')[2].Split('&')[0];
                    var amount = decryptedQueryString.Split('=')[3];

                    IPaymentDataService paymentDataService = new PaymentDataService();
                    IPolicyDataProvider policyDataProvider = new PolicyDataProvider();
                                                            
                    //if transaction key not exists, add record into database
                    if (!policyDataProvider.GetPolicyPaymentIdByTransactionCode(transactionId))
                    {
                        IPaymentService paymentService = new PaymentService(guardServiceProvider);
                        var user = UserSession();
                        paymentService.AddPayment(new Payment { Amount = Convert.ToDecimal(amount), AgencyCode = pdResponse.PolicyDetails.Agency.Code, PolicyCode = policyNumber, PaymentDate = DateTime.Now, UserId = user.Email });

                        paymentDataService.AddPaymentDetails(new PolicyPaymentDetailDTO
                        {
                            PolicyID = policyDataProvider.GetPolicyIdByPolicyNumber(policyNumber),
                            AmountPaid = Convert.ToDecimal(amount),
                            IsActive = true,
                            TransactionCode = transactionId,
                            CreatedBy = user.Id,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = user.Id,
                            ModifiedDate = DateTime.Now
                        });

                        transactionID = transactionId;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// update session values, when user change payment option
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePaymentOption(UpdatePayment postData)
        {
            Session["PaymentOption"] = SetDefaultPayOption(GetPayOptions(), postData.selectedId);
            Session["PaymentAmount"] = postData.amount;

            return Json(new
            {
                status = true,
                JsonRequestBehavior.AllowGet
            });
        }

        [HttpPost]
        public JsonResult GetPolicyDetailByPolicyNumber(string CYBKey, string transactionCode)
        {
            try
            {
                var decryptObject = DecryptedCYBKey(CYBKey);
                string policyCode = decryptObject.PolicyCode;
                string businessName = decryptObject.BusinessName;
                string transactionID = string.Empty;
                string transactionError = string.Empty;

                //Call the Policy Details API to get inception,expiration, status etc...
                PolicyDetailsResponse policyDetailsResponse = _policyDetailsService.GetPolicyDetails(new PolicyDetailsRequestParms() { PolicyCode = policyCode, UserId = UserSession().Email });

                if (!string.IsNullOrEmpty(transactionCode))
                {
                    if (transactionCode != "undefined")
                        if (!AddPaymentDetails(transactionCode, ref transactionID, policyDetailsResponse))
                        {
                            transactionError = "We will get back to you within next 24 hours with policy detail.";
                        }
                }
                //For getting the Description of business type
                ((BusinessTypeService)_businessTypeService).ServiceProvider = guardServiceProvider;
                _businessTypeResponse = _businessTypeService.GetBusinessTypes(new BusinessTypeRequestParms() { InsuredNameTypesOnly = true });

                // Calls the Quotes API Service to get the info for mailing adress...               
                Quote quote = _quoteService.GetQuote(new QuoteRequestParms
                {
                    PolicyId = policyCode,
                    IncludeRelatedInsuredNames = true,
                    IncludeRelatedOfficers = false,
                    IncludeRelatedLocations = true,
                    IncludeRelatedExposuresGraph = true,
                    IncludeRelatedContactsGraph = false,
                    IncludeRelatedPolicyData = false,
                    IncludeRelatedRatingData = false,
                    IncludeRelatedPaymentTerms = false,
                    IncludeRelatedQuestions = false,
                    IncludeRelatedQuoteStatus = false
                });
                //getting the Exact business description
                var businessType = (_businessTypeResponse.Join(quote.InsuredNames, x1 => x1.BusinessTypeCode, x2 => x2.NameType, (x1, x2) => x1)).ToList();

                BillingSummaryResponse billingSummaryResponse = _billingSummaryService.GetBillingSummary(new BillingSummaryRequestParms() { PolicyCode = policyCode, UserId = UserSession().Email });

                //update values and set default option, after successful payment
                if (!policyDetailsResponse.OperationStatus.RequestProcessed)
                {
                    return Json(new { status = false, errorMessage = policyDetailsResponse.OperationStatus.Messages.FirstOrDefault().Text }, JsonRequestBehavior.AllowGet);
                }

                if (!billingSummaryResponse.OperationStatus.RequestProcessed)
                {
                    return Json(new { status = false, errorMessage = billingSummaryResponse.OperationStatus.Messages.FirstOrDefault().Text }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = true,
                        errorMessage = "",
                        policy = new
                        {
                            code = policyDetailsResponse.PolicyDetails.PolicyCode,
                            effectiveDate = policyDetailsResponse.PolicyDetails.PolicyBegin.ToString("MM/dd/yyyy"),
                            expiredDate = policyDetailsResponse.PolicyDetails.PolicyExpires.ToString("MM/dd/yyyy"),
                            statusText = _dashboardService.GetPolicyStatus(policyDetailsResponse.PolicyDetails),
                            status = policyDetailsResponse.PolicyDetails.IsActive,
                            totalPremiumPaid = policyDetailsResponse.PolicyDetails.IsActive ? 0 : billingSummaryResponse.BillingSummary.TotalPaid,
                            minimumAmountDue = billingSummaryResponse.BillingSummary.CurrentDue,
                            remainingBalance = billingSummaryResponse.BillingSummary.AccountBalance,
                            transactionID, transactionError, 
                            businessName,
                            businessType = businessType.Count > 0 ? businessType[0].Description : "",
                            address1 = quote.Locations.Count > 0 ? quote.Locations[0].Addr1 : "",
                            address2 = quote.Locations.Count > 0 ? quote.Locations[0].Addr2 : "",
                            city = quote.Locations.Count > 0 ? quote.Locations[0].City : "",
                            state = quote.Locations.Count > 0 ? quote.Locations[0].State : "",
                            zip = quote.Locations.Count > 0 ? quote.Locations[0].Zip : ""
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DecryptPolicyData(string CYBKey)
        {
            try
            {
                PolicyInformation policyInformation = DecryptedCYBKey(CYBKey);
                return Json(new { status = true, policyInformation }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { status = false, redirectStatus = true, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
