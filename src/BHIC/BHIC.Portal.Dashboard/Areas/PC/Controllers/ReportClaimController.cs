using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BHIC.Common.Mailing;
using BHIC.Common.XmlHelper;
using BHIC.Domain.Dashboard;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.CommonUtilities;
using BHIC.Common;
using System.Web;
using System.Reflection;
using BHIC.Core.PolicyCentre;
using System.IO.Compression;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class ReportClaimController : BaseController
    {
        private static readonly List<string> Uploadedfiles = new List<string>();

        //Get: /ReportClaim/RequestReportClaimFromHome/
        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult ReportClaimDashboard()
        {
            return PartialView("ReportClaimDashboard");
        }


        [AllowAnonymous]
        [CustomAntiForgeryToken]
        public ActionResult ReportClaimFromHome()
        {
            return PartialView("ReportClaimFromHome");
        }

        //Post: /ReportClaim/RequestReportClaimFromHome/
        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult RequestReportClaimFromHome(ReportClaim postData)
        {
            try
            {
                //Model validation
                string modelErrors = string.Empty;
                if (!Helper_ValidateModel_OutSidePC(postData))
                {
                    return Json(new { success = false, errorMessage = modelErrors }, JsonRequestBehavior.AllowGet);
                }

                if (ModelState.IsValid)
                {
                    return ((SendEmail(postData)) ? Json(new { success = true }, JsonRequestBehavior.AllowGet)
                                : Json(new { success = false, errorMessage = "Report a Claim failed" }));
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, redirectStatus = true, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize]
        public ActionResult ReportClaim()
        {
            return PartialView("ReportClaim");
        }

        [CustomAuthorize]
        [CustomAntiForgeryToken]
        public ActionResult ReportClaimFromHeader()
        {
            return PartialView("ReportClaimFromHeader");
        }

        //Post: /ReportClaim/RequestReportClaim/
        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult RequestReportClaim(ReportClaim postData)
        {
            try
            {
                //Model validation
                string modelErrors = string.Empty;
                PolicyInformation policyInformation = DecryptedCYBKey(postData.policyCode);
                postData.policyCode = policyInformation.PolicyCode;
                postData.UserEmail = policyInformation.PolicyUserContact.PolicyEmail;
                if (!Helper_ValidateModel(postData, policyInformation, ref modelErrors))
                {
                    return Json(new { success = false, errorMessage = modelErrors }, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    return ((SendEmail(postData)) ? Json(new { success = true }, JsonRequestBehavior.AllowGet)
                                : Json(new { success = false, errorMessage = "Report a Claim failed." }));
                }
                else
                {
                    return Json(new { success = false, errorMessage = GetModelError() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, redirectStatus = true, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult UploadDocuments(string pCode, string CYBKey)
        {
            var files = Enumerable.Range(0, Request.Files.Count).Select(i => Request.Files[i]);
            try
            {
                if (Request.Files.Count <= ConfigCommonKeyReader.MaxFileCount)
                {
                    string policyCode = pCode ?? DecryptedCYBKey(CYBKey).PolicyCode;
                    policyCode = (policyCode == "undefined" || string.IsNullOrWhiteSpace(policyCode)) ? Convert.ToString(Guid.NewGuid()) : policyCode;
                    bool isFilePosted = false;
                    foreach (HttpPostedFileBase file in files)
                    {
                        isFilePosted = true;
                        string fileName;
                        if (file.FileName.Contains("\\"))
                        {
                            var tempFilePath = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            fileName = tempFilePath[tempFilePath.Length - 1];
                        }
                        else
                        {
                            fileName = file.FileName;
                        }

                        string filePath = Path.Combine(ConfigCommonKeyReader.UploadFiles, policyCode, "ClaimDocuments", fileName);
                        if (Path.GetDirectoryName(filePath).Length > 0 && !Directory.Exists(Path.GetDirectoryName(filePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        }
                        if (filePath.Contains(".zip"))
                        {
                            using (Stream inputFileStream = new MemoryStream())
                            {
                                file.InputStream.CopyTo(inputFileStream);
                                FileUploadValidationStaus fileUploadValidationStaus = IsZipContentValid(inputFileStream, filePath);
                                if (!fileUploadValidationStaus.IsValid)
                                {
                                    return Json(new { success = false, errorMessage = fileUploadValidationStaus.ValidationMessages }, JsonRequestBehavior.AllowGet);
                                }

                                using (FileStream outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    file.InputStream.EncryptFile(outFile, policyCode);
                                    Uploadedfiles.Add(filePath);
                                }
                            }
                        }
                        else
                        {
                            FileUploadValidationStaus fileUploadValidationStaus = UploadHelper.ValidateUploadedFiles(file, filePath);
                            if (fileUploadValidationStaus.IsValid)
                            {
                                using (FileStream outFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                {
                                    file.InputStream.EncryptFile(outFile, policyCode);
                                    Uploadedfiles.Add(filePath);
                                }
                            }
                            else
                            {
                                // Throw error
                                return Json(new { success = false, errorMessage = fileUploadValidationStaus.ValidationMessages }, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }

                    if (isFilePosted)
                    {
                        return Json(new { success = true, policyCode = policyCode }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "Something went wrong. Please try again." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, errorMessage = string.Format("Maximum {0} files can be uploaded.", ConfigCommonKeyReader.MaxFileCount) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                loggingService.Fatal(string.Format("Method {0} executed with error message : {1}", MethodBase.GetCurrentMethod().Name, ex.ToString()));
                return Json(new { success = false, errorMessage = "Something went wrong." }, JsonRequestBehavior.AllowGet);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFileStream"></param>
        /// <returns></returns>
        private FileUploadValidationStaus IsZipContentValid(Stream inputFileStream, string filePath)
        {
            FileUploadValidationStaus fileUploadValidationStaus = new FileUploadValidationStaus();
            using (ZipArchive archive = new ZipArchive(inputFileStream))
            {
                List<ZipArchiveEntry> allEntries = archive.Entries.ToList();
                foreach (ZipArchiveEntry entry in allEntries)
                {
                    if (entry.FullName.Contains(".zip"))
                    {
                        var stream = entry.Open();
                        fileUploadValidationStaus = IsZipContentValid(stream, filePath);
                        if (!fileUploadValidationStaus.IsValid)
                        {
                            return fileUploadValidationStaus;
                        }
                    }

                    if (entry.FullName.Contains('.'))
                    {
                        using (MemoryStream fileMemoryStream = new MemoryStream())
                        {
                            entry.Open().CopyTo(fileMemoryStream);
                            fileUploadValidationStaus = UploadHelper.ValidateUploadedFiles(fileMemoryStream, filePath, entry.FullName);
                            if (!fileUploadValidationStaus.IsValid)
                            {
                                return fileUploadValidationStaus;
                            }
                        }
                    }
                }
            }
            fileUploadValidationStaus.IsValid = true;
            return fileUploadValidationStaus;
        }

        /// <summary>
        /// Priavate method used for sending a mail.
        /// </summary>
        /// <param name="reportClaim">ReportClaim object</param>
        /// <returns>bool</returns>
        [CustomAntiForgeryToken]
        private bool SendEmail(ReportClaim reportClaim)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            MailMsg mailMsg;
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            DateTime defaultDate = DateTime.Parse("01/01/0001");
            string dateValue = "";
            dateValue = (reportClaim.dateOfIllness.ToShortDateString() == defaultDate.ToShortDateString()) ? null : reportClaim.dateOfIllness.ToShortDateString();

            if (reportClaim.ClaimType.Equals("BP", StringComparison.OrdinalIgnoreCase))
            {
                model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClaimRequestEmailTo));
                model.Add("MailSubject", "useremail-policyclaim-subject");
                model.Add("MailBody", "useremail-bop-policyclaim-body");
                model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
                model.Add("AbsoulteURL", GetEmailImageUrl());
                model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
                model.Add("PolicyCode", GetClaimPolicyCode(reportClaim.policyCode));
                model.Add("ClaimType", "BOP");
                model.Add("DateOfLoss", dateValue);
                model.Add("BusinessName", reportClaim.NameOfBusiness);
                model.Add("YourName", reportClaim.YourName);
                model.Add("BestContactNumber", reportClaim.PhoneNumber);
                model.Add("PolicyClaimEmail", reportClaim.UserEmail);
                model.Add("InjuredWorkerName", reportClaim.NameOfInjuredWorker);
                model.Add("Location", reportClaim.location);
                model.Add("Description", reportClaim.description);
                model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
                model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
                model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
                model.Add("ClaimsProcessingEmailText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["ClaimsProcessing_Email"]));
                model.Add("ClaimsProcessingEmail", string.Concat("mailto:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["ClaimsProcessing_Email"])));

                mailMsg = MailTemplateBuilder.MailBOPReportClaim(model);
            }
            else
            {
                model.Add("ClientEmailID", String.Join(";", ConfigCommonKeyReader.ClaimRequestEmailTo));
                model.Add("MailSubject", "useremail-policyclaim-subject");
                model.Add("MailBody", "useremail-policyclaim-body");
                model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
                model.Add("AbsoulteURL", GetEmailImageUrl());
                model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
                model.Add("PolicyCode", GetClaimPolicyCode(reportClaim.policyCode));
                model.Add("ClaimType", "WC");
                model.Add("DateOfIllness", dateValue);
                model.Add("PolicyClaimEmail", reportClaim.UserEmail);
                model.Add("BusinessName", reportClaim.NameOfBusiness);
                model.Add("YourName", reportClaim.YourName);
                model.Add("BestContactNumber", reportClaim.PhoneNumber);
                model.Add("InjuredWorkerName", reportClaim.NameOfInjuredWorker);
                model.Add("Location", reportClaim.location);
                model.Add("Description", reportClaim.description);
                model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
                model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
                model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
                model.Add("ClaimsProcessingEmailText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["ClaimsProcessing_Email"]));
                model.Add("ClaimsProcessingEmail", string.Concat("mailto:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["ClaimsProcessing_Email"])));

                mailMsg = MailTemplateBuilder.MailReportClaim(model);
            }


            try
            {
                if (Uploadedfiles.Count > 0)
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.SendMailMessage(mailMsg.SenderEmailAddr, mailMsg.RecipEmailAddr, mailMsg.Cc, mailMsg.Bcc, mailMsg.Subject, mailMsg.MessageBody, Uploadedfiles, true, reportClaim.policyCode);
                    Uploadedfiles.Clear();
                }
                else
                {
                    MailTemplateBuilder.SendMail(mailMsg);
                }

            }
            catch 
            {
                Uploadedfiles.Clear();
                throw;
            }
            return true;
        }


        private bool Helper_ValidateModel(ReportClaim postdata, PolicyInformation policyinfo, ref string errorMessage)
        {
            bool isModelValid = true;
            DateTime requestedDate = UtilityFunctions.ConvertToDate(postdata.EffectiveDate);

            if (requestedDate != new DateTime())
            {
                postdata.dateOfIllness = requestedDate;
                if (Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) > Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy")) || Convert.ToDateTime(requestedDate.ToString("MM/dd/yyyy")) < Convert.ToDateTime(policyinfo.EffectiveDate.ToString("MM/dd/yyyy")))
                {
                    errorMessage = "Date of Illness is not valid";
                    isModelValid = false;
                }
            }
            if (postdata.ClaimType.Equals("BP", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.Remove("NameOfInjuredWorker");
                postdata.NameOfInjuredWorker = "";
            }
            return isModelValid;
        }

        private bool Helper_ValidateModel_OutSidePC(ReportClaim postDate)
        {
            bool isModelValid = true;

            try
            {
                ModelState.Remove("policyCode");
                ModelState.Remove("NameOfInjuredWorker");
                ModelState.Remove("dateOfIllness");
                ModelState.Remove("EffectiveDate");
                ModelState.Remove("location");
                ModelState.Remove("description");
            }
            catch (Exception)
            {
                isModelValid = false;
            }
            return isModelValid;
        }

        /// <summary>
        /// Validate whether passed value is valid GUID
        /// </summary>
        /// <param name="policyCode"></param>
        /// <returns></returns>
        private string GetClaimPolicyCode(string policyCode)
        {
            Guid newGuid;
            if (Guid.TryParse(policyCode, out newGuid))
            {
                return string.Empty;
            }
            else
            {
                return policyCode;
            }
        }
    }
}