using System;
using System.Web.Mvc;
using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.Domain.Dashboard;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Core.PolicyCentre;
using BHIC.Common;
using System.Collections.Generic;
using System.Reflection;
using BHIC.Common.Config;
using BHIC.Common.Client;
using BHIC.Core.Policy;
using BHIC.Domain.Policy;
using BHIC.Contract.Policy;
using BHIC.Domain.Service;
using System.Linq;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    [CustomAuthorize]
    [CustomAntiForgeryToken]
    public class EditContactInfoController : BaseController
    {
        private static ILoggingService _logger = LoggingService.Instance;

        // GET: /PC/EditContactInfo/
        public ActionResult EditContactInfo()
        {
            return PartialView("EditContactInfo");
        }

        [HttpPost]
        public ActionResult UpadteContactInfo(ContactInformation postData)
        {
            try
            {
                ModelState.Clear();
                TryValidateModel(postData);
                if (ModelState.IsValid)
                {
                    //Decrypt the Encrypted values
                    PolicyInformation policyInformation = DecryptedCYBKey(postData.PolicyCode);
                    string policyCode = policyInformation.PolicyCode;
                    int? contactId = Convert.ToInt32(Encryption.DecryptText(postData.ContactId)); ;
                    int? phoneId = Convert.ToInt32(Encryption.DecryptText(postData.PhoneId));
                    int quoteId = Convert.ToInt32(Encryption.DecryptText(postData.QuoteId));

                    //Post Cancellation Request to Guard API
                    IContactService contactService = new ContactService(guardServiceProvider);
                    Phone phone = new Phone
                    {
                        ContactId = contactId,
                        PhoneId = phoneId,
                        PhoneType = Encryption.DecryptText(postData.PhoneType)
                    };
                    if (postData.PhoneNumber.ToString().Length > 10)
                    {
                        phone.PhoneNumber = postData.PhoneNumber.ToString().Substring(0, 10);
                        phone.Extension = postData.PhoneNumber.ToString().Substring(10, postData.PhoneNumber.ToString().Length - 10);
                    }
                    else
                    {
                        phone.PhoneNumber = postData.PhoneNumber.ToString();
                    }
                    Contact contactdata = new Contact
                    {
                        ContactId = phone.ContactId,
                        Name = postData.Name,
                        Email = postData.Email,
                        QuoteId = quoteId,
                        ContactType = "Misc"
                    };

                    contactdata.Phones.Add(phone);
                    OperationStatus operationStatus = contactService.AddContact(contactdata);
                    if (operationStatus.RequestSuccessful)
                    {
                        //update CYBKey encrypted data when change the user contact info 
                        policyInformation.PolicyUserContact.UserName = postData.Name;
                        policyInformation.PolicyUserContact.PolicyEmail = postData.Email;
                        policyInformation.PolicyUserContact.PolicyContactNumber = Convert.ToString(postData.PhoneNumber);

                        string CYBKey = EncryptCYBKey(policyInformation);

                        //Send email to guard
                        if (MailTemplateBuilder.SendMail(MailTemplateBuilder.MailEditContactInfo(GetEmailModel(postData, policyCode))))
                        {
                            return Json(new { success = true, user = postData, successMessage = "Contact Information Updated.", CYBKey }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, errorMessage = "Error occurred while sending mail." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Contact Information not updated." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = Constants.ExceptionMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        // Comment: Get Email Model ready for Contact Information
        private Dictionary<string, string> GetEmailModel(ContactInformation postData, string policyCode)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClientEmailIdAddressChangeRequest));
            model.Add("MailSubject", "useremail-contactinfo-subject");
            model.Add("MailBody", "useremail-contactinfo-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("ContactInfoName", postData.Name);
            model.Add("ContactInfoPhoneNumber", postData.PhoneNumber.ToString());
            model.Add("ContactInfoEmail", postData.Email);
            model.Add("PolicyCode", policyCode);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("SupportEmailTextSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"]);
            model.Add("SupportEmailHrefSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"]);

            return model;
        }

        // Comment: Get contact Information for a policy from Quote API
        [HttpPost]
        public JsonResult GetContactInformation(string CYBKey)
        {
            try
            {
                var decryptObject = DecryptedCYBKey(CYBKey);
                string policyCode = decryptObject.PolicyCode;
                string policyStatus = decryptObject.Status;

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
                    IncludeRelatedLocations = false,
                    IncludeRelatedContactsGraph = true,
                    IncludeRelatedQuestions = false,
                    IncludeRelatedQuoteStatus = false
                }, false);

                if (quote.Contacts.Count > 0)
                {
                    var contacts = quote.Contacts.FirstOrDefault(t => t.ContactType == "Misc");
                    var phones = contacts.Phones.FirstOrDefault(t => t.PhoneType == "H") == null ? contacts.Phones.FirstOrDefault(t => t.PhoneType == "B") : contacts.Phones.FirstOrDefault(t => t.PhoneType == "H");

                    return Json(new { success = true, user = new { QuoteId = Encryption.EncryptText(contacts.QuoteId.ToString()), ContactId = Encryption.EncryptText(contacts.ContactId.ToString()), PhoneId = Encryption.EncryptText(phones.PhoneId.ToString()), PhoneType = Encryption.EncryptText(phones.PhoneType.ToString()), Name = contacts.Name.Trim(), PhoneNumber = phones.PhoneNumber.Trim() + (string.IsNullOrEmpty(phones.Extension) ? "" : phones.Extension.Trim()), EmailID = contacts.Email.Trim(), status = policyStatus } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, user = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = "Something went wrong." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
