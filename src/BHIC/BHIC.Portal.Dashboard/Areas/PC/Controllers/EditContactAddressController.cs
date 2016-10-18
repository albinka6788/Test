using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Contract.PolicyCentre;
using BHIC.Core.Background;
using BHIC.Core.Policy;
using BHIC.Core.PolicyCentre;
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class EditContactAddressController : BaseController
    {
        //
        // GET: /PC/EditContactAddress/

        public ActionResult EditContactAddress()
        {
            return PartialView("EditContactAddress");
        }

        [HttpPost]
        public JsonResult GetAddress(string CYBKey, string contactType)
        {
            try
            {
                if (string.IsNullOrEmpty(CYBKey))
                {
                    throw new Exception("CYBKey is not supplied");
                }
                var decryptObject = DecryptedCYBKey(CYBKey);
                string policyCode = decryptObject.PolicyCode;

                if (contactType.ToUpper() != "A")
                {
                    QuoteService quoteService = new QuoteService(guardServiceProvider);
                    var quote = quoteService.GetQuote(new QuoteRequestParms
                            {
                                PolicyId = policyCode,
                                IncludeRelatedPolicyData = false,
                                IncludeRelatedRatingData = false,
                                IncludeRelatedPaymentTerms = false,
                                IncludeRelatedInsuredNames = false,
                                IncludeRelatedExposuresGraph = false,
                                IncludeRelatedOfficers = false,
                                IncludeRelatedLocations = true,
                                IncludeRelatedContactsGraph = true,
                                IncludeRelatedQuestions = false,
                                IncludeRelatedQuoteStatus = false
                            }, false);
                    if (quote.Locations.Count > 0)
                    {
                        for (int counter = 0; counter < quote.Locations.Count(); counter++)
                        {
                            if (quote.Locations[counter].LocationType.ToUpper() == contactType.ToUpper())
                            {
                                return Json(new
                                {
                                    success = true,
                                    contactAddress = new
                                    {
                                        CYBKey,
                                        PolicyStatus = decryptObject.Status,
                                        PolicyCode = decryptObject.PolicyCode,
                                        Addr1 = quote.Locations[counter].Addr1,
                                        Addr2 = quote.Locations[counter].Addr2,
                                        City = quote.Locations[counter].City,
                                        State = quote.Locations[counter].State,
                                        Zip = quote.Locations[counter].Zip,
                                    }
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    contactAddress = new
                    {
                        CYBKey,
                        PolicyStatus = decryptObject.Status,
                        PolicyCode = decryptObject.PolicyCode,
                        Addr1 = "",
                        Addr2 = "",
                        City = "",
                        State = "",
                        Zip = ""
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateContactAddress(AddressInformation postData)
        {
            try
            {
                postData.Email = UserSession().Email;
                postData.UserPhone = UserSession().PhoneNumber;
                ModelState.Clear();
                TryValidateModel(postData);
                if (ModelState.IsValid)
                {
                    ICityStateZipCodeSearch cityStateZipcode = new CityStateZipCodeSearch(guardServiceProvider);
                    bool validzipcode = cityStateZipcode.SearchCityStateZipCode(
                            new VCityStateZipCodeRequestParms { City = postData.City, State = postData.State, ZipCode = postData.ZipCode });
                    if (validzipcode)
                    {
                        for (int i = 0; i < postData.PolicyCode.Length; i++)
                        {
                            List<string> cybPolicyDetail = Encryption.DecryptText(postData.PolicyCode[i]).Split(new string[] { "|:|" }, StringSplitOptions.None).ToList();
                            postData.Email = cybPolicyDetail[6];
                            postData.UserPhone = Convert.ToInt64(cybPolicyDetail[7]);

                            if (UserSession().Email.Equals(cybPolicyDetail[2]) && !cybPolicyDetail[1].ToUpper().Equals("NO COVERAGE") && !cybPolicyDetail[1].ToUpper().Equals("EXPIRED") && !cybPolicyDetail[1].ToUpper().Equals("CANCELLED"))
                            {
                                postData.PolicyCode[i] = cybPolicyDetail[0];
                            }
                            else
                            {
                                return Json(new { success = false, errorMessage = "Please enter valid information" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailEditContactAddress(GetEmailModel(postData, string.Join(", ", postData.PolicyCode)))))
                        {
                            return Json(new { success = true, successMessage = "Contact address updated Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { success = false, errorMessage = "Please enter valid information" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                if (ex.Message.Contains("ERROR: Invalid City"))
                {
                    var errmsg = ex.Message.Split(':')[1];
                    errmsg = errmsg.Split('<')[0];
                    return Json(new { success = false, errorMessage = errmsg }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<string, string> GetEmailModel(AddressInformation postData, string policies)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClientEmailIdAddressChangeRequest));
            model.Add("MailSubject", "useremail-contactAddress-subject");
            model.Add("MailBody", "useremail-contactAddress-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("ContactInfoPhoneNumber", postData.UserPhone.ToString());
            model.Add("ContactInfoEmail", postData.Email);
            model.Add("BusinessName", "");
            model.Add("AddressType", postData.AddressType);
            model.Add("Policycodes", policies);
            model.Add("Address1", postData.Address1);
            model.Add("Address2", postData.Address2);
            model.Add("City", postData.City);
            model.Add("State", postData.State);
            model.Add("ZipCode", postData.ZipCode);
            model.Add("Additional", postData.Additional);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("SupportEmailTextSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"]);
            model.Add("SupportEmailHrefSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"]);

            return model;
        }

        [HttpGet]
        public JsonResult GetCurrentUserData()
        {
            string firstName = UserSession().FirstName;
            string lastName = UserSession().LastName;
            long? phoneNumber = UserSession().PhoneNumber;
            string emailId = UserSession().Email;
            string organizationName = UserSession().OrganizationName;
            return Json(new { success = true, user = new { FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, EmailID = emailId, OrganizationName = organizationName } }, JsonRequestBehavior.AllowGet);
        }
    }
}
