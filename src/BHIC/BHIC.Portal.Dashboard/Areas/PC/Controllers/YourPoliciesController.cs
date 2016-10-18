using BHIC.Common;
using BHIC.Common.Logging;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.Policy;
using BHIC.Core.PolicyCentre;
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
    public class YourPoliciesController : BaseController
    {
        #region variables
        private IUserPolicyDataService _userPolicyDataService;
        private List<PolicyDetailsResponse> _policyDetailsResponse;
        private static readonly ILoggingService Logger = LoggingService.Instance;
        #endregion

        #region PartialView for YourPolicies
        //
        // GET: /PC/YourPolicies/
        public ActionResult YourPolicies()
        {
            return PartialView("YourPolicies");
        }
        #endregion

        #region Load the Policies in Dashboard
        [HttpGet]
        public ActionResult GetYourPolicies()
        {
            List<PolicyDetails> policyDetails = new List<PolicyDetails>();
            QuoteService quoteService = new QuoteService(guardServiceProvider);
            UserRegistration user = null;
            try
            {
                user = UserSession();
                if (user.Id > 0)
                {
                    //Comment: Get the list of policies from Database based on Email Id
                    DashboardService dashboardService = new DashboardService();
                    var lstPolicies = dashboardService.GetUserPoliciesFromDB(user.Email);
                    List<String> lstGuardPolicies = lstPolicies.Where(a => a.LOB == 1 || a.LOB == 2).Select(a => a.PolicyCode).ToList();

                    //Comment: Get the policy details from the Guard API for the fetched policies
                    if (lstPolicies != null && lstPolicies.Count > 0)
                    {
                        _userPolicyDataService = new UserPolicyDataService(guardServiceProvider);
                        _policyDetailsResponse = _userPolicyDataService.GetPolicyList(lstGuardPolicies, UserSession().Email, guardServiceProvider);
                        var policyResponse = _policyDetailsResponse.Where(t => t.OperationStatus.RequestSuccessful).ToList();
                        if (policyResponse.Count > 0)
                        {
                            //Preparing list of policy code
                            List<string> lstPolicyNumber = policyResponse.Select(item => item.PolicyDetails.PolicyCode).ToList();

                            //call batch action for contact detail
                            List<Quote> bulkQuoteResponse = quoteService.GetContactFromQuoteList(lstPolicyNumber);

                            foreach (var item in policyResponse)
                            {
                                Quote quote = new Quote();
                                foreach (var quoteResponse in bulkQuoteResponse)
                                {
                                    if (quoteResponse.PolicyData.MgaCode == item.PolicyDetails.PolicyCode)
                                    {
                                        quote = quoteResponse;
                                        break;
                                    }
                                }

                                //Get the status for the given policy
                                item.PolicyDetails.Status = dashboardService.GetPolicyStatus(item.PolicyDetails);
                                item.PolicyDetails.LOB = lstPolicies.Where(a => a.PolicyCode == item.PolicyDetails.PolicyCode).Select(a => a.LOB).FirstOrDefault();
                                item.PolicyDetails.Insured.Name = item.PolicyDetails.Insured.Name.Trim();

                                //Encrypt the CYBKey to be used throughout the application
                                if (quote.Contacts.Count > 0)
                                {                                    
                                    item.PolicyDetails.CYBPolicyNumber = Encryption.EncryptText(string.Join("|:|", item.PolicyDetails.PolicyCode, item.PolicyDetails.Status, user.Email, item.PolicyDetails.LOB, item.PolicyDetails.Insured.Name.Trim(), quote.Contacts[0].Name.Trim(),
                                                                                    quote.Contacts[0].Email.Trim(), quote.Contacts[0].Phones[0].PhoneNumber.Trim() + (string.IsNullOrEmpty(quote.Contacts[0].Phones[0].Extension) ? "" : quote.Contacts[0].Phones[0].Extension.Trim()),
                                                                                    item.PolicyDetails.PolicyBegin, item.PolicyDetails.PolicyExpires));
                                }
                                else
                                {

                                    item.PolicyDetails.CYBPolicyNumber = Encryption.EncryptText(string.Join("|:|", item.PolicyDetails.PolicyCode, item.PolicyDetails.Status, user.Email, item.PolicyDetails.LOB, item.PolicyDetails.Insured.Name.Trim(), string.Empty,
                                                                                    string.Empty, string.Empty, item.PolicyDetails.PolicyBegin, item.PolicyDetails.PolicyExpires));                               
                                }

                                policyDetails.Add(item.PolicyDetails);

                            }
                            return Json(new { success = true, user, yourPolicies = policyDetails }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    Logger.Info(string.Format("YourPolicies/GetYourPolicies Session user expired : {0},{1}", user.Id, user.Email));
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
            }

            return Json(new { success = false, user, yourPolicies = policyDetails }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check whether Session Updated
        [NonAction]
        public static void UpdateUserSession()
        {
            try
            {
                DashboardService obj = new DashboardService();
                UserRegistration user = (UserRegistration)System.Web.HttpContext.Current.Session["user"];
                user = obj.GetUserDetails(user);
                System.Web.HttpContext.Current.Session["user"] = user;
            }
            catch
            {
                // ignored
            }
        }
        #endregion

        #region Get Policy Specific User Details
        public JsonResult GetPolicySpecificUserDetail(string CYBKey)
        {
            PolicyInformation decryptedKey = DecryptedCYBKey(CYBKey);
            return Json(new
            {
                status = true,
                user = new
                {
                    Name = decryptedKey.PolicyUserContact.UserName,
                    PhoneNumber = decryptedKey.PolicyUserContact.PolicyContactNumber,
                    Email = decryptedKey.PolicyUserContact.PolicyEmail
                },
                ExpiryDate = decryptedKey.ExpiryDate
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
