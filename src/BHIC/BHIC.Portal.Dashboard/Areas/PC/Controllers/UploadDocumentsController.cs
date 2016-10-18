using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BHIC.Domain.Dashboard;
using BHIC.Common;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Mailing;
using BHIC.Common.XmlHelper;
using BHIC.Core.PolicyCentre;
using BHIC.Portal.Dashboard.App_Start;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Core.Background;

namespace BHIC.Portal.Dashboard.Areas.PC.Controllers
{
    public class UploadDocumentsController : BaseController
    {

        private static Common.Logging.ILoggingService _logger = BHIC.Common.Logging.LoggingService.Instance;
        private static readonly List<string> Uploadedfiles = new List<string>();

        // GET: /PC/UploadDocuments/
        [CustomAuthorize]
        [CustomAntiForgeryToken]
        public ActionResult UploadDocuments()
        {
            return PartialView("UploadDocuments");
        }

        //Post: /ReportClaim/RequestReportClaim/
        [HttpPost]
        [CustomAntiForgeryToken]        
        public ActionResult RequestUploadDocuments(UploadDocumentDTO postData)
        {
            try
            {
                //Model validation
                PolicyInformation policyInformation = DecryptedCYBKey(postData.policyCode);
                postData.policyCode = policyInformation.PolicyCode;
                postData.UserEmail = policyInformation.PolicyUserContact.PolicyEmail; 
                postData.UserPhone = policyInformation.PolicyUserContact.PolicyContactNumber;
                postData.BusinessName = policyInformation.BusinessName; 
                ModelState.Clear();
                TryValidateModel(postData);
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
        public ActionResult UploadDocuments(string CYBKey)
        {
            var files = Enumerable.Range(0, Request.Files.Count).Select(i => Request.Files[i]);
            try
            {
                if (Request.Files.Count <= ConfigCommonKeyReader.MaxFileCount)
                {
                    string policyCode = DecryptedCYBKey(CYBKey).PolicyCode;
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

                        string filePath = Path.Combine(ConfigCommonKeyReader.UploadFiles, policyCode, "PolicyDocuments", fileName);
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
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
        /// <param name="uploadDoc">Upload Documents object</param>
        /// <returns>bool</returns>
        [CustomAntiForgeryToken]
        private bool SendEmail(UploadDocumentDTO uploadDoc)
        {
            Dictionary<string, string> model = new Dictionary<string, string>();
            ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };
            ISystemVariableService systemVariable = new SystemVariableService(guardServiceProvider);

            model.Add("MailSubject", "useremail-uploaddocument-subject");
            model.Add("MailBody", "useremail-uploaddocument-body");
            model.Add("BaseUrl", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]));
            model.Add("AbsoulteURL", GetEmailImageUrl());
            model.Add("IPAddress", GetUser_IP(this.ControllerContext.HttpContext));
            model.Add("ContactNumber", uploadDoc.UserPhone);
            model.Add("UserEmail", uploadDoc.UserEmail);
            model.Add("PolicyCode", uploadDoc.policyCode);
            model.Add("Description", uploadDoc.description);
            model.Add("BusinessName", uploadDoc.BusinessName);
            model.Add("SupportPhoneNumber", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"]));
            model.Add("SupportPhoneNumberHref", string.Concat("tel:", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["PhoneNumber_CSR"])));
            model.Add("WebsiteUrlText", systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]));
            model.Add("SupportEmailTextSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"]);
            model.Add("SupportEmailHrefSalesSupport", ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"]);

            var mailMsg = MailTemplateBuilder.MailUploadDocuments(model);
            if (Uploadedfiles.Count > 0)
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMailMessage(null, mailMsg.RecipEmailAddr, mailMsg.Cc, mailMsg.Bcc, mailMsg.Subject, mailMsg.MessageBody, Uploadedfiles, true, uploadDoc.policyCode);
                Uploadedfiles.Clear();
            }
            else
            {
                MailTemplateBuilder.SendMail(mailMsg);
            }
            return true;
        }

    }
}
