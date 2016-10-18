#region Using Directives

using System;

using BHIC.Common.Client;
using System.Linq;
using BHIC.Common.Mailing;
using BHIC.Contract.PurchasePath;
using BHIC.ViewDomain;
using BHIC.ViewDomain.QuestionEngine;
using System.Collections.Generic;
using BHIC.Common;
using BHIC.Common.Configuration;
using BHIC.Common.XmlHelper;
using BHIC.Common.Logging;
using BHIC.Domain.PurchasePath;
using BHIC.ViewDomain.Landing;
using System.Text;
using BHIC.Contract.Background;
using BHIC.Core.Background;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class ReferralQuote : IReferralQuote
    {
        #region Variables : Class-Level local variables decalration

        protected static string LineOfBusiness = "WC";

        CustomSession appSession;
        ServiceProvider serviceProvider;

        #endregion

        #region Constructors

        public ReferralQuote() { }

        /// <summary>
        /// Initilize local instance of custom-session object to be used in different methods in this BLL
        /// </summary>
        /// <param name="customAppSession"></param>
        public ReferralQuote(CustomSession customAppSession, ServiceProvider commonServiceProvider)
        {
            appSession = customAppSession;
            serviceProvider = commonServiceProvider;
        }

        #endregion

        #region Main methods

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="referralReasons"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        public bool ProcessSoftReferralMailNew(ReferralQuoteMailViewModel referralQuoteVM, List<string> listOfUploadedFiles)
        {
            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("referralMailSubject", ThemeManager.ThemeSharedContent("guardemail-soft-referral-subject") ?? string.Empty);
            mailContentModel.Add("referralMailBody", ThemeManager.ThemeSharedContent("guardemail-soft-referral-body") ?? string.Empty);

            #endregion

            #region Comment : Here get QuoteVM object from application custom session

            ISystemVariableService systemVariable = new SystemVariableService(serviceProvider);

            string referralReason = string.Empty, referralDescription = string.Empty, declineReason = string.Empty;
            StringBuilder sbReferralReasons = new StringBuilder(), sbReferralDescriptions = new StringBuilder();
            StringBuilder sbFinalReferralReason = new StringBuilder(), sbFinalReferralDescriptions = new StringBuilder();
            string xModValueMessage = string.Empty;

            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here if any quote/questionnaire object found empty then stop further execution of code below
            if (sessionQuoteVM.IsNull() || sessionQuestionnaireVM.IsNull())
            {
                LoggingService.Instance.Fatal("Error occurred while sending referral mail session object not found !");
                return false;
            }
            else
            {
                #region Comment : Here First of all check referral scenario-id must >0 otherwise raise error

                if (sessionQuestionnaireVM.ReferralHistory.ReferralScenarioIdsList.Count <= 0)
                {
                    LoggingService.Instance.Fatal("Error occurred because no referral scenario found !");
                    return false;
                }

                #endregion

                #region Comment : Here Iterate all the Scenarios and concatenate all reasons to show "Mutiple Reasons" in referral mail

                var referralHisotry = sessionQuestionnaireVM.ReferralHistory;

                if (referralHisotry.ReferralScenarioIdsList.Any())
                {
                    //Comment : Here first check for no. of items existance in ReferralHistory based on that take following action
                    #region Comment : Here if History have "SINGLE" referral reason in list then

                    if (referralHisotry.ReferralScenarioIdsList.Count == 1)
                    {
                        //Comment": Here set Referral/Decline Reasons/Descirptions
                        if (referralHisotry.ReferralScenarioTextList.Any())
                        {
                            var priorReferralData = referralHisotry.ReferralScenarioTextList.First();

                            sbFinalReferralReason.Append(priorReferralData.ReasonsList.Any() ? string.Join(";", priorReferralData.ReasonsList) : string.Empty);
                            sbFinalReferralDescriptions.Append(priorReferralData.DescriptionList.Any() ? string.Join(";", priorReferralData.DescriptionList) : string.Empty);

                            //Message for XMod value
                            xModValueMessage = referralHisotry.XModValueMessage;

                            declineReason = string.Empty;
                        }
                    }

                    #endregion

                    #region Comment : Here if History have "MULTIPLE" referral reason in list then

                    if (referralHisotry.ReferralScenarioIdsList.Count > 1)
                    {
                        #region Comment : Here As per requirement "Cases where this cycle repeats for multiple no. of times then just first reason and current, if any"

                        #region Comment : Here first get "PRIOR Reasons & Descriptions"

                        //Comment : Here first try to figure-out that "Is that first reason was due to DECLINE in referral history"
                        var priorReferralDeclineIds = referralHisotry.ReferralScenarioIdsList.First();
                        var priorReferralData = referralHisotry.ReferralScenarioTextList.First();

                        if (priorReferralDeclineIds != null && priorReferralDeclineIds.Any())
                        {
                            //Referral reasons scenario ids (10, 11, 12, 13) are decline reason
                            if (priorReferralDeclineIds.Intersect(new List<int> { 10, 11, 12, 13 }).Any())
                            {
                                #region Comment : Here confirms that first time user was "DECLINED" in ReferralHistory then set "PriorReasons"

                                //Reset while iteration
                                referralReason = string.Empty; referralDescription = string.Empty;

                                //Comment : Here call generic method to get specific scenario based Referral-Reason/Description

                                //Comment : Here [GUIN-312-Prem] As per requirement added "Prior Reason"
                                declineReason = string.Format("<b>Prior reason:</b> {{{0}}}", priorReferralData.ReasonsList.Any()
                                    ? (string.Join(";", priorReferralDeclineIds.Contains(13) ? priorReferralData.DescriptionList : priorReferralData.ReasonsList))
                                    : string.Empty);

                                //declineReason = priorReferralData.ReasonsList.Any() ? string.Join(";", priorReferralData.ReasonsList) : string.Empty; //Old Line

                                #endregion
                            }
                            else
                            {
                                #region Comment : Here confirms that first time user was "REFER" in ReferralHistory then set "PriorReasons"

                                //Reset while iteration
                                referralReason = string.Empty; referralDescription = string.Empty;

                                //Comment : Here RESET xmod related to default 
                                xModValueMessage = string.Empty;

                                sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}",
                                    priorReferralData.ReasonsList.Any() ? string.Join(";", priorReferralData.ReasonsList) : string.Empty);

                                sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}}",
                                    priorReferralData.DescriptionList.Any() ? string.Join(";", priorReferralData.DescriptionList) : string.Empty);

                                #endregion
                            }

                        }

                        #endregion

                        #region Comment : Here second get "CURRENT Reasons & Descriptions"

                        //Comment : Here second try to figure-out "Current reasons.desciption from referral history"
                        var currentReferralDeclineIds = referralHisotry.ReferralScenarioIdsList.Last();
                        var currentReferralData = referralHisotry.ReferralScenarioTextList.Last();

                        if (currentReferralDeclineIds != null && currentReferralDeclineIds.Any())
                        {
                            #region Comment : Here confirms that first time user was "REFER" in ReferralHistory then set "PriorReasons"

                            //Reset
                            sbReferralReasons = new StringBuilder(); sbReferralDescriptions = new StringBuilder();
                            referralReason = string.Empty; referralDescription = string.Empty;

                            //Comment : Here get all current referral reason & descriptions
                            sbReferralReasons.Append(currentReferralData.ReasonsList.Any() ? string.Join(";", currentReferralData.ReasonsList) : string.Empty);
                            sbReferralDescriptions.Append(currentReferralData.DescriptionList.Any() ? string.Join(";", currentReferralData.DescriptionList) : string.Empty);

                            //Message for XMod value
                            xModValueMessage = referralHisotry.XModValueMessage;

                            //Comment : Here if last reason equals to ScenarioId-9 then only change "ReferralReason" particular word e.g. "Referral or Decline"
                            //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                            if (sbReferralReasons.ToString().IndexOf("##") > -1 && currentReferralDeclineIds.Contains(9))
                            {
                                //Comment : Here if "DeclineReason" exists then only set "Decline" in final status text
                                var previousReferralStatus = UtilityFunctions.FillTemplateWithModelValues(sbReferralReasons.ToString()
                                    , new { ReferralStatus = (string.IsNullOrEmpty(declineReason) ? "referral" : "decline") });

                                sbReferralReasons = new StringBuilder();
                                sbReferralReasons.Append(previousReferralStatus);
                            }

                            //Comment : Here according ADAM new inputs don't include only show the Prior Reason Current Reason if there actually was a ‘prior reason’"
                            if (sbFinalReferralReason.Length == 0 && sbFinalReferralDescriptions.Length == 0)
                            {
                                //Comment : Here in case of "DECLINE" Reason must show prior-reason,description even those will be blank (On Neelam suggestion on 31.05.2016)
                                if (!string.IsNullOrEmpty(declineReason))
                                {
                                    sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}, <b>Current reason:</b> {{{1}}}", string.Empty, sbReferralReasons.ToString());

                                    //Comment : Here [GUIN-312-Prem] Don't show these details in case of "Decline"
                                    if (!string.IsNullOrEmpty(sbReferralDescriptions.ToString()))
                                    {
                                        sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}} , <b>Current description:</b> {{{1}}}", string.Empty, sbReferralDescriptions.ToString());
                                    }
                                }
                                else
                                {
                                    sbFinalReferralReason.Append(sbReferralReasons.ToString());
                                    sbFinalReferralDescriptions.Append(sbReferralDescriptions.ToString());
                                }
                            }
                            else
                            {
                                sbFinalReferralReason.AppendFormat(", <b>Current reason:</b> {{{0}}}", sbReferralReasons.ToString());
                                sbFinalReferralDescriptions.AppendFormat(", <b>Current description:</b> {{{0}}}", sbReferralDescriptions.ToString());
                            }

                            #endregion
                        }

                        #endregion

                        #endregion
                    }

                    #endregion
                }

                #endregion
            }

            #endregion

            #region Comment : Here prepare template populated with data

            var model = new ReferralQuoteMailViewModel
            {
                //Comment : Here STEP-1. Basic mail template communication information
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"] ?? string.Empty,
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                //WebsiteUrlHref = ConfigCommonKeyReader.SchemeAndHostURL ?? string.Empty,
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"] ?? string.Empty,
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"] ?? string.Empty,
                SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"] ?? string.Empty,
                SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"] ?? string.Empty,

                //Comment : Here Additional instructions information
                ReferralAgyValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralAgyValue"] ?? string.Empty,
                ReferralBranchValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralBranchValue"] ?? string.Empty,
                ReferralLeadSourceValue = ConfigCommonKeyReader.HostURL,

                //Comment : Here STEP-2. QuoteStatus information
                ReferralStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralStatus"] ?? string.Empty,
                ReferralReason = sbFinalReferralReason.ToString(),
                DeclineReason = declineReason,
                ReferralDescription = sbFinalReferralDescriptions.ToString(),
                ReferralImportStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralImportStatus"] ?? string.Empty,

                //Comment : Here STEP-3. GeneralPolicy information
                StateCode = appSession.StateAbbr,
                //logic is if "keyword-search-id" found then its search by profession
                BusinessLookupType = (sessionQuoteVM.SelectedSearch == 0) ? "Keyword" : "Dropdown",
                //logic is if It's "Keyword Search" then use BusienssName otherwise "other-class-description"
                ClassDescription = GetReferralClassDescription(sessionQuoteVM),
                //ClassDescription = 
                //(!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? (sessionQuoteVM.BusinessName ?? string.Empty) : (sessionQuoteVM.OtherClassDesc ?? string.Empty),

                //Logic if "Other" is selected then PolicyData will not be available (To pick InceptionDate value)
                PolicyInceptionDate = !sessionQuoteVM.PolicyData.IsNull() ? sessionQuoteVM.PolicyData.InceptionDate.Value.ToString("MM/dd/yyyy")
                                        : DateTime.Parse(sessionQuoteVM.InceptionDate, new System.Globalization.CultureInfo("en-US", true)).ToString("MM/dd/yyyy"),
                AnnualPayroll = sessionQuoteVM.TotalPayroll.ToString(),
                BusinessYears = !sessionQuoteVM.SelectedYearInBusiness.IsNull() ? sessionQuoteVM.SelectedYearInBusiness.text.ToString() : string.Empty,
                FeinOrSSNumber = (!appSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber)) ? appSession.QuestionnaireVM.TaxIdNumber : "NA",
                XModValueMessage = xModValueMessage,

                //Comment : Here STEP-4. Include Class information
                ClassInformationHtml = referralQuoteVM.ClassInformationHtml,

                //Comment : Here STEP-5. Include QuestionAndResponse information
                //QuestionsAndResponsesHtml = !string.IsNullOrEmpty(sbQuestionsAndResponsesHtml.ToString()) ? sbQuestionsAndResponsesHtml.ToString() : string.Empty,
                QuestionsAndResponsesHtml = referralQuoteVM.QuestionsAndResponsesHtml,

                //Comment : Here STEP-6. Referral page contact details
                QuoteId = referralQuoteVM.QuoteId,
                ContactName = referralQuoteVM.ContactName,
                BusinessName = referralQuoteVM.BusinessName,
                PhoneNumber = referralQuoteVM.PhoneNumber,
                Email = referralQuoteVM.Email,
                QuoteReferralMessage = referralQuoteVM.QuoteReferralMessage,
                LOB = LineOfBusiness,
                ZipCode = appSession.ZipCode,
                EstimatedTotalPremium = !string.IsNullOrEmpty(referralQuoteVM.EstimatedTotalPremium) ? referralQuoteVM.EstimatedTotalPremium : "NA"
            };

            //Comment : Here build template and send mail
            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSoftReferral(model, mailContentModel);

            #endregion

            #region Comment : Here mail configuration and send mail to concern authority

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = ConfigCommonKeyReader.ReferralEmailTo;
            // UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailSalesSupport"]);
            //mailMsg.Cc = UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

            mailMsg.Cc = ConfigCommonKeyReader.ReferralEmailCc;
            mailMsg.Bcc = ConfigCommonKeyReader.ReferralEmailBcc;
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ReferralEmailFrom;

            //Send user mail
            var mailSentStatus = mailMsg != null ?
                //If attachement then accordingly
                (listOfUploadedFiles.Count > 0) ?
                    buildTemplate.SendMailWithAttachments(mailMsg, listOfUploadedFiles, true, referralQuoteVM.QuoteId.ToString()) :
                    buildTemplate.SendMail(mailMsg)
                : false;


            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="referralReasons"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        public bool ProcessSoftReferralMail(ReferralQuoteMailViewModel referralQuoteVM, ReferralReasons referralReasons, List<string> listOfUploadedFiles)
        {
            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("referralMailSubject", ThemeManager.ThemeSharedContent("guardemail-soft-referral-subject") ?? string.Empty);
            mailContentModel.Add("referralMailBody", ThemeManager.ThemeSharedContent("guardemail-soft-referral-body") ?? string.Empty);

            #endregion

            #region Comment : Here get QuoteVM object from application custom session

            ISystemVariableService systemVariable = new SystemVariableService(serviceProvider);

            string referralReason = string.Empty, referralDescription = string.Empty, declineReason = string.Empty;
            StringBuilder sbReferralReasons = new StringBuilder(), sbReferralDescriptions = new StringBuilder();
            StringBuilder sbFinalReferralReason = new StringBuilder(), sbFinalReferralDescriptions = new StringBuilder();
            string xModValueMessage = string.Empty;

            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here if any quote/questionnaire object found empty then stop further execution of code below
            if (sessionQuoteVM.IsNull() || sessionQuestionnaireVM.IsNull() || (referralReasons.IsNull() || referralReasons.ReferralReason.Count == 0))
            {
                LoggingService.Instance.Fatal("Error occurred while sending referral mail session object not found !");
                return false;
            }
            else
            {
                #region Comment : Here First of all check referral scenario-id must >0 otherwise raise error

                if (sessionQuestionnaireVM.ReferralScenarioIds.Count <= 0)
                {
                    LoggingService.Instance.Fatal("Error occurred because no referral scenario found !");
                    return false;
                }

                #endregion

                #region Comment : Here Iterate all the Scenarios and concatenate all reasons to show "Mutiple Reasons" in referral mail

                if (sessionQuestionnaireVM.ReferralScenariosHistory.Count > 0)
                {
                    //Comment : Here first check for no. of items existance in ReferralHistory based on that take following action
                    #region Comment : Here if History have "SINGLE" referral reason in list then

                    if (sessionQuestionnaireVM.ReferralScenariosHistory.Count == 1)
                    {
                        //Comment : Here get all current referral reason & descriptions
                        GetAllReferralReasons(sessionQuestionnaireVM.ReferralScenariosHistory.First(), referralReasons, out sbReferralReasons, out sbReferralDescriptions, out xModValueMessage);

                        //Comment": Here set Referral/Decline Reasons/Descirptions
                        sbFinalReferralReason.Append(sbReferralReasons.ToString());
                        sbFinalReferralDescriptions.Append(sbReferralDescriptions.ToString());

                        //sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}, <b>Current reason:</b> {{{1}}}", string.Empty, sbReferralReasons.ToString());
                        //sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}}, <b>Current description:</b> {{{1}}}", string.Empty, sbReferralDescriptions.ToString());

                        declineReason = string.Empty;
                    }

                    #endregion

                    #region Comment : Here if History have "MULTIPLE" referral reason in list then

                    if (sessionQuestionnaireVM.ReferralScenariosHistory.Count > 1)
                    {
                        #region Comment : Here As per requirement "Cases where this cycle repeats for multiple no. of times then just first reason and current, if any"

                        #region Comment : Here first get "PRIOR Reasons & Descriptions"

                        //Comment : Here first try to figure-out that "Is that first reason was due to DECLINE in referral history"
                        var priorReferralDeclineReasons = sessionQuestionnaireVM.ReferralScenariosHistory.First();

                        if (priorReferralDeclineReasons != null && priorReferralDeclineReasons.Count > 0)
                        {
                            //Referral reasons scenario ids (10, 11, 12, 13) are decline reason
                            if (priorReferralDeclineReasons.Intersect(new List<int> { 10, 11, 12, 13 }).Any())
                            {
                                #region Comment : Here confirms that first time user was "DECLINED" in ReferralHistory then set "PriorReasons"

                                //sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}", string.Empty);
                                //sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}}", string.Empty);

                                //Reset while iteration
                                referralReason = string.Empty; referralDescription = string.Empty;

                                //Comment : Here call generic method to get specific scenario based Referral-Reason/Description
                                GetReferralReasonAndDescription(priorReferralDeclineReasons.First(), referralReasons, out referralReason, out referralDescription);
                                declineReason = referralReason;

                                #endregion
                            }
                            else
                            {
                                #region Comment : Here confirms that first time user was "REFER" in ReferralHistory then set "PriorReasons"

                                //Reset while iteration
                                referralReason = string.Empty; referralDescription = string.Empty;

                                //Comment : Here get all current referral reason & descriptions
                                GetAllReferralReasons(sessionQuestionnaireVM.ReferralScenariosHistory.First(), referralReasons, out sbReferralReasons, out sbReferralDescriptions, out xModValueMessage);

                                //Comment : Here RESET xmod related to default 
                                xModValueMessage = string.Empty;

                                sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}", sbReferralReasons.ToString());
                                sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}}", sbReferralDescriptions.ToString());

                                #endregion
                            }

                        }

                        #endregion

                        #region Comment : Here second get "CURRENT Reasons & Descriptions"

                        //Comment : Here second try to figure-out "Current reasons.desciption from referral history"
                        var currentReferralDeclineReasons = sessionQuestionnaireVM.ReferralScenariosHistory.Last();

                        if (currentReferralDeclineReasons != null && currentReferralDeclineReasons.Count > 0)
                        {
                            #region Comment : Here confirms that first time user was "REFER" in ReferralHistory then set "PriorReasons"

                            //Reset
                            sbReferralReasons = new StringBuilder(); sbReferralDescriptions = new StringBuilder();
                            referralReason = string.Empty; referralDescription = string.Empty;

                            //Comment : Here get all current referral reason & descriptions
                            GetAllReferralReasons(currentReferralDeclineReasons, referralReasons, out sbReferralReasons, out sbReferralDescriptions, out xModValueMessage);

                            //Comment : Here if last reason equals to ScenarioId-9 then only change "ReferralReason" particular word e.g. "Referral or Decline"
                            //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                            if (sbReferralReasons.ToString().IndexOf("##") > -1 && currentReferralDeclineReasons.Contains(9))
                            {
                                //Comment : Here if "DeclineReason" exists then only set "Decline" in final status text
                                var previousReferralStatus = UtilityFunctions.FillTemplateWithModelValues(sbReferralReasons.ToString()
                                    , new { ReferralStatus = (string.IsNullOrEmpty(declineReason) ? "referral" : "decline") });

                                sbReferralReasons = new StringBuilder();
                                sbReferralReasons.Append(previousReferralStatus);
                            }

                            //Comment : Here according ADAM new inputs don't include only show the Prior Reason Current Reason if there actually was a ‘prior reason’"
                            if (sbFinalReferralReason.Length == 0 && sbFinalReferralDescriptions.Length == 0)
                            {
                                //Comment : Here in case of "DECLINE" Reason must show prior-reason,description even those will be blank (On Neelam suggestion on 31.05.2016)
                                if (!string.IsNullOrEmpty(declineReason))
                                {
                                    sbFinalReferralReason.AppendFormat("<b>Prior reason:</b> {{{0}}}, <b>Current reason:</b> {{{1}}}", string.Empty, sbReferralReasons.ToString());
                                    sbFinalReferralDescriptions.AppendFormat("<b>Prior description:</b> {{{0}}} , <b>Current description:</b> {{{1}}}", string.Empty, sbReferralDescriptions.ToString());
                                }
                                else
                                {
                                    sbFinalReferralReason.Append(sbReferralReasons.ToString());
                                    sbFinalReferralDescriptions.Append(sbReferralDescriptions.ToString());
                                }
                            }
                            else
                            {
                                sbFinalReferralReason.AppendFormat(", <b>Current reason:</b> {{{0}}}", sbReferralReasons.ToString());
                                sbFinalReferralDescriptions.AppendFormat(", <b>Current description:</b> {{{0}}}", sbReferralDescriptions.ToString());
                            }

                            #endregion
                        }

                        #endregion

                        #endregion
                    }

                    #endregion
                }

                #endregion
            }

            #endregion

            #region Comment : Here prepare template populated with data

            var model = new ReferralQuoteMailViewModel
            {
                //Comment : Here STEP-1. Basic mail template communication information
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"] ?? string.Empty,
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                //WebsiteUrlHref = ConfigCommonKeyReader.SchemeAndHostURL ?? string.Empty,
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"] ?? string.Empty,
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"] ?? string.Empty,
                SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"] ?? string.Empty,
                SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"] ?? string.Empty,

                //Comment : Here Additional instructions information
                ReferralAgyValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralAgyValue"] ?? string.Empty,
                ReferralBranchValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralBranchValue"] ?? string.Empty,
                ReferralLeadSourceValue = ConfigCommonKeyReader.HostURL,

                //Comment : Here STEP-2. QuoteStatus information
                ReferralStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralStatus"] ?? string.Empty,
                ReferralReason = sbFinalReferralReason.ToString(),
                DeclineReason = declineReason,
                ReferralDescription = sbFinalReferralDescriptions.ToString(),
                ReferralImportStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralImportStatus"] ?? string.Empty,

                //Comment : Here STEP-3. GeneralPolicy information
                StateCode = appSession.StateAbbr,
                //logic is if "keyword-search-id" found then its search by profession
                BusinessLookupType = (sessionQuoteVM.SelectedSearch == 0) ? "Keyword" : "Dropdown",
                //logic is if It's "Keyword Search" then use BusienssName otherwise "other-class-description"
                ClassDescription = GetReferralClassDescription(sessionQuoteVM),
                //ClassDescription = 
                //(!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? (sessionQuoteVM.BusinessName ?? string.Empty) : (sessionQuoteVM.OtherClassDesc ?? string.Empty),

                //Logic if "Other" is selected then PolicyData will not be available (To pick InceptionDate value)
                PolicyInceptionDate = !sessionQuoteVM.PolicyData.IsNull() ? sessionQuoteVM.PolicyData.InceptionDate.Value.ToString("MM/dd/yyyy")
                                        : DateTime.Parse(sessionQuoteVM.InceptionDate, new System.Globalization.CultureInfo("en-US", true)).ToString("MM/dd/yyyy"),
                AnnualPayroll = sessionQuoteVM.TotalPayroll.ToString(),
                BusinessYears = !sessionQuoteVM.SelectedYearInBusiness.IsNull() ? sessionQuoteVM.SelectedYearInBusiness.text.ToString() : string.Empty,
                FeinOrSSNumber = (!appSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber)) ? appSession.QuestionnaireVM.TaxIdNumber : "NA",
                XModValueMessage = xModValueMessage,

                //Comment : Here STEP-4. Include Class information
                ClassInformationHtml = referralQuoteVM.ClassInformationHtml,

                //Comment : Here STEP-5. Include QuestionAndResponse information
                //QuestionsAndResponsesHtml = !string.IsNullOrEmpty(sbQuestionsAndResponsesHtml.ToString()) ? sbQuestionsAndResponsesHtml.ToString() : string.Empty,
                QuestionsAndResponsesHtml = referralQuoteVM.QuestionsAndResponsesHtml,

                //Comment : Here STEP-6. Referral page contact details
                QuoteId = referralQuoteVM.QuoteId,
                ContactName = referralQuoteVM.ContactName,
                BusinessName = referralQuoteVM.BusinessName,
                PhoneNumber = referralQuoteVM.PhoneNumber,
                Email = referralQuoteVM.Email,
                QuoteReferralMessage = referralQuoteVM.QuoteReferralMessage,
                LOB = LineOfBusiness,
                ZipCode = appSession.ZipCode,
                EstimatedTotalPremium = !string.IsNullOrEmpty(referralQuoteVM.EstimatedTotalPremium) ? referralQuoteVM.EstimatedTotalPremium : "NA"
            };

            //Comment : Here build template and send mail
            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSoftReferral(model, mailContentModel);

            #endregion

            #region Comment : Here mail configuration and send mail to concern authority

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = ConfigCommonKeyReader.ReferralEmailTo;
            // UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailSalesSupport"]);
            //mailMsg.Cc = UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

            mailMsg.Cc = ConfigCommonKeyReader.ReferralEmailCc;
            mailMsg.Bcc = ConfigCommonKeyReader.ReferralEmailBcc;
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ReferralEmailFrom;

            //Send user mail
            var mailSentStatus = mailMsg != null ?
                //If attachement then accordingly
                (listOfUploadedFiles.Count > 0) ?
                    buildTemplate.SendMailWithAttachments(mailMsg, listOfUploadedFiles, true, referralQuoteVM.QuoteId.ToString()) :
                    buildTemplate.SendMail(mailMsg)
                : false;


            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="referralReasons"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        public bool ProcessSoftReferralMailOLD(ReferralQuoteMailViewModel referralQuoteVM, ReferralReasons referralReasons, List<string> listOfUploadedFiles)
        {
            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("referralMailSubject", ThemeManager.ThemeSharedContent("guardemail-soft-referral-subject") ?? string.Empty);
            mailContentModel.Add("referralMailBody", ThemeManager.ThemeSharedContent("guardemail-soft-referral-body") ?? string.Empty);

            #endregion

            #region Comment : Here get QuoteVM object from application custom session


            ISystemVariableService systemVariable = new SystemVariableService(serviceProvider);

            string referralReason = string.Empty, referralDescription = string.Empty;
            StringBuilder sbReferralReasons = new StringBuilder(), sbReferralDescriptions = new StringBuilder();
            string xModValueMessage = string.Empty;
            //int referralScenarioId;

            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here if any quote/questionnaire object found empty then stop further execution of code below
            if (sessionQuoteVM.IsNull() || sessionQuestionnaireVM.IsNull() || (referralReasons.IsNull() || referralReasons.ReferralReason.Count == 0))
            {
                LoggingService.Instance.Fatal("Error occurred while sending referral mail session object not found !");
                return false;
            }
            else
            {
                #region Comment : Here First of all check referral scenario-id must >0 otherwise raise error

                if (sessionQuestionnaireVM.ReferralScenarioIds.Count <= 0)
                {
                    LoggingService.Instance.Fatal("Error occurred because no referral scenario found !");
                    return false;
                }

                #endregion

                #region Comment : Here Iterate all the Scenarios and concatenate all reasons to show "Mutiple Reasons" in referral mail

                if (sessionQuestionnaireVM.ReferralScenarioIds.Count > 0)
                {
                    foreach (var referralScenarioId in sessionQuestionnaireVM.ReferralScenarioIds)
                    {
                        //Reset while iteration
                        referralReason = string.Empty; referralDescription = string.Empty;

                        //Comment : Here call generic method to get specific scenario based Referral-Reason/Description
                        GetReferralReasonAndDescription(referralScenarioId, referralReasons, out referralReason, out referralDescription);

                        #region Comment : Here SCENARIO-General. FEIN/XMod flow related logics

                        appSession.QuestionnaireVM.TaxIdNumber = appSession.QuestionnaireVM.TaxIdNumber;

                        //Comment : Here in case XMod factor check all following scenarios                    
                        //If xMOD flow is not applicable
                        //If xMOD flow is applicable and a value is returned
                        //If xMOD flow is applicable and no value is returned
                        //If xMOD flow is applicable and an error message is returned

                        //Comment : Here SCENARIO-6.1 If xMOD flow is "not applicable" then 
                        if (!appSession.QuestionnaireVM.IsNull() && string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber))
                        {
                            xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage1"] ?? string.Empty;
                        }
                        else
                        {
                            #region Comment : Here Scenarios where FEIN applicable

                            //Comment : Here SCENARIO-6.2 If xMOD flow is applicable and a value is returned
                            if (appSession.QuestionnaireVM.XModValue > 0)
                            {
                                xModValueMessage = Math.Round(appSession.QuestionnaireVM.XModValue, 3).ToString("0.000");
                            }
                            //Comment : Here SCENARIO-6.3 If xMOD flow is applicable and no value is returned
                            else if (referralScenarioId != 6 && appSession.QuestionnaireVM.XModValue == 0)
                            {
                                xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage2"] ?? string.Empty;
                            }

                            #endregion
                        }

                        #endregion

                        #region Comment : Here SCENARIO-1. Other Classification

                        //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                        if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 1)
                        {
                            referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { OtherClassName = sessionQuoteVM.OtherClassDesc });

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion

                            //if found then no need of further below code execution
                            continue;
                        }

                        #endregion

                        #region Comment : Here SCENARIO-2. Busienss class DirectSales='E'

                        //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                        if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 2)
                        {
                            referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { ReferralOnlyClass = sessionQuoteVM.BusinessName });

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion

                            //if found then no need of further below code execution
                            continue;
                        }

                        #endregion

                        #region Comment : Here SCENARIO-3. Busienss class Minimum Payroll validation along with GoodState/BadState logic

                        //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                        if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 3)
                        {
                            referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                                new { GoodStateAcceptableMinPayroll = sessionQuoteVM.MinExpValidationAmount });

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion

                            //if found then no need of further below code execution
                            continue;
                        }

                        #endregion

                        #region Comment : Here SCENARIO-4. Multi-State selection by user

                        //Comment : Here if it's MS scenario
                        if (referralScenarioId == 4)
                        {
                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion
                        }

                        #endregion

                        #region Comment : Here SCENARIO-5. Companion Minimum % validation failed

                        //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                        if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 5)
                        {
                            #region Comment : Here becuase we have maintained the MinimumPayrollThreshold % for primary/companion classes

                            //Comment : Here ReferralQuote interface refernce to do/make process all business logic
                            ICaptureQuote captureQuoteBLL = new CaptureQuote();

                            BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuoteBLL.GetMinimumPayrollThreshold(appSession.StateAbbr, sessionQuoteVM.ClassDescriptionId.Value);

                            //if primaryClass data found
                            if (!primaryClassCodeData.IsNull())
                            {
                                //Comment : Here calculate user entered payroll %
                                var userEnteredPayrollPercentage = Math.Round
                                    (
                                        ((Math.Round(sessionQuoteVM.AnnualPayroll, 2) / Math.Round(Convert.ToDecimal(sessionQuoteVM.TotalPayroll), 2)) * 100)
                                        , 0
                                    ).ToString();

                                referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                                new
                                {
                                    UserEnteredPayrollPercentage = userEnteredPayrollPercentage
                                    ,
                                    ClassAcceptablePayrollPercentage = Math.Round(Convert.ToDecimal(primaryClassCodeData.MinimumPayrollThreshold * 100), 0).ToString()
                                });
                            }

                            #endregion

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion

                            //if found then no need of further below code execution
                            continue;
                        }

                        #endregion

                        #region Comment : Here SCENARIO-6. FEIN/XMod flow related logics

                        //Comment : Here SCENARIO-6.4 If xMOD flow is applicable and an error message is returned
                        if (!appSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber) && referralScenarioId == 6)
                        {
                            //same as referral-description
                            xModValueMessage = referralDescription;

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion
                        }

                        #endregion

                        #region Comment : Here SCENARIO-7. Past 3 years Claims Frequency

                        //Comment : Here if it's MS scenario
                        if (referralScenarioId == 7)
                        {
                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion
                        }

                        #endregion

                        #region Comment : Here SCENARIO-8. Guard rating-engine returned Referral

                        //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                        if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 8)
                        {
                            referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                                , new { GuardReferralMessage = sessionQuestionnaireVM.QuestionsResponse.ResultMessages });

                            #region Comment : Here "Multiple Reasons" will be added during every iteration

                            //Comment : Here club all reasons one by one
                            if (!string.IsNullOrEmpty(referralReason))
                            {
                                sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                                sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                            }

                            //Comment : Here trace important flag/objects into log
                            LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                            #endregion

                            //if found then no need of further below code execution
                            continue;
                        }

                        #endregion
                    }
                }

                #endregion
            }

            #endregion

            #region Comment : Here prepare template populated with data

            var model = new ReferralQuoteMailViewModel
            {
                //Comment : Here STEP-1. Basic mail template communication information
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"] ?? string.Empty,
                WebsiteUrlText = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_Domain"]),
                WebsiteUrlHref = systemVariable.GetSystemVariableByKey(ConfigCommonKeyReader.ApplicationContactInfo["Company_WebsiteShortURL"]),
                //WebsiteUrlHref = ConfigCommonKeyReader.SchemeAndHostURL ?? string.Empty,
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"] ?? string.Empty,
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"] ?? string.Empty,
                SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"] ?? string.Empty,
                SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"] ?? string.Empty,

                //Comment : Here Additional instructions information
                ReferralAgyValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralAgyValue"] ?? string.Empty,
                ReferralBranchValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralBranchValue"] ?? string.Empty,
                ReferralLeadSourceValue = ConfigCommonKeyReader.HostURL,

                //Comment : Here STEP-2. QuoteStatus information
                ReferralStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralStatus"] ?? string.Empty,
                ReferralReason = sbReferralReasons.ToString(),
                ReferralDescription = sbReferralDescriptions.ToString(),
                ReferralImportStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralImportStatus"] ?? string.Empty,

                //Comment : Here STEP-3. GeneralPolicy information
                StateCode = appSession.StateAbbr,
                //logic is if "keyword-search-id" found then its search by profession
                BusinessLookupType = (!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? "Keyword" : "Dropdown",
                //logic is if It's "Keyword Search" then use BusienssName otherwise "other-class-description"
                ClassDescription = GetReferralClassDescription(sessionQuoteVM),
                //ClassDescription = 
                //(!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? (sessionQuoteVM.BusinessName ?? string.Empty) : (sessionQuoteVM.OtherClassDesc ?? string.Empty),
                PolicyInceptionDate = sessionQuoteVM.PolicyData.InceptionDate.Value.ToString("MM/dd/yyyy") ?? string.Empty,
                AnnualPayroll = sessionQuoteVM.TotalPayroll.ToString(),
                BusinessYears = sessionQuoteVM.BusinessYearsText,
                FeinOrSSNumber = !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber) ? appSession.QuestionnaireVM.TaxIdNumber : "NA",
                XModValueMessage = xModValueMessage,

                //Comment : Here STEP-4. Include Class information
                ClassInformationHtml = referralQuoteVM.ClassInformationHtml,

                //Comment : Here STEP-5. Include QuestionAndResponse information
                //QuestionsAndResponsesHtml = !string.IsNullOrEmpty(sbQuestionsAndResponsesHtml.ToString()) ? sbQuestionsAndResponsesHtml.ToString() : string.Empty,
                QuestionsAndResponsesHtml = referralQuoteVM.QuestionsAndResponsesHtml,

                //Comment : Here STEP-6. Referral page contact details
                QuoteId = referralQuoteVM.QuoteId,
                ContactName = referralQuoteVM.ContactName,
                BusinessName = referralQuoteVM.BusinessName,
                PhoneNumber = referralQuoteVM.PhoneNumber,
                Email = referralQuoteVM.Email,
                QuoteReferralMessage = referralQuoteVM.QuoteReferralMessage,
                LOB = LineOfBusiness,
                ZipCode = appSession.ZipCode,
                AbsoulteURL = referralQuoteVM.AbsoulteURL
            };

            //Comment : Here build template and send mail
            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSoftReferral(model, mailContentModel);

            #endregion

            #region Comment : Here mail configuration and send mail to concern authority

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = ConfigCommonKeyReader.ReferralEmailTo;
            // UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailSalesSupport"]);
            //mailMsg.Cc = UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

            mailMsg.Cc = ConfigCommonKeyReader.ReferralEmailCc;
            mailMsg.Bcc = ConfigCommonKeyReader.ReferralEmailBcc;
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ReferralEmailFrom;

            //Send user mail
            var mailSentStatus = mailMsg != null ?
                //If attachement then accordingly
                (listOfUploadedFiles.Count > 0) ?
                    buildTemplate.SendMailWithAttachments(mailMsg, listOfUploadedFiles, true, referralQuoteVM.QuoteId.ToString()) :
                    buildTemplate.SendMail(mailMsg)
                : false;


            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// Prepare mail content static and dynamically for SoftReferral quote new process
        /// </summary>
        /// <param name="referralQuoteVM"></param>
        /// <param name="referralReasons"></param>
        /// <param name="listOfUploadedFiles"></param>
        /// <returns></returns>
        public bool ProcessSoftReferralMailOLDER(ReferralQuoteMailViewModel referralQuoteVM, ReferralReasons referralReasons, List<string> listOfUploadedFiles)
        {
            #region Comment : Here prepare mail object model using ThemeManager and shared pre-defined templates

            Dictionary<string, object> mailContentModel = new Dictionary<string, object>();
            mailContentModel.Add("referralMailSubject", ThemeManager.ThemeSharedContent("guardemail-soft-referral-subject") ?? string.Empty);
            mailContentModel.Add("referralMailBody", ThemeManager.ThemeSharedContent("guardemail-soft-referral-body") ?? string.Empty);

            #endregion

            #region Comment : Here get QuoteVM object from application custom session

            string referralReason = string.Empty, referralDescription = string.Empty;
            string xModValueMessage = string.Empty;
            int referralScenarioId;

            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;

            //Comment : Here if any quote/questionnaire object found empty then stop further execution of code below
            if (sessionQuoteVM.IsNull() || sessionQuestionnaireVM.IsNull() || (referralReasons.IsNull() || referralReasons.ReferralReason.Count == 0))
            {
                LoggingService.Instance.Fatal("Error occurred while sending referral mail session object not found !");
                return false;
            }
            else
            {
                #region Comment : Here First of all check referral scenario-id must >0 otherwise raise error

                if (sessionQuestionnaireVM.ReferralScenarioId <= 0)
                {
                    LoggingService.Instance.Fatal("Error occurred because no referral scenario found !");
                    return false;
                }
                else
                {
                    referralScenarioId = sessionQuestionnaireVM.ReferralScenarioId;
                }

                #endregion

                #region Comment : Here Scenario-1. "Other Classification"

                if (referralScenarioId > 0)
                {
                    //Comment : Here call generic method to get specific scenario based Referral-Reason/Description
                    GetReferralReasonAndDescription(referralScenarioId, referralReasons, out referralReason, out referralDescription);

                    #region Comment : Here SCENARIO-1. Other Classification

                    //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                    if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 1)
                    {
                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { OtherClassName = sessionQuoteVM.OtherClassDesc });
                    }

                    #endregion

                    #region Comment : Here SCENARIO-2. Busienss class DirectSales='E'

                    //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                    if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 2)
                    {
                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { ReferralOnlyClass = sessionQuoteVM.BusinessName });
                    }

                    #endregion

                    #region Comment : Here SCENARIO-3. Busienss class Minimum Payroll validation along with GoodState/BadState logic

                    //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                    if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 3)
                    {
                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                            new { GoodStateAcceptableMinPayroll = sessionQuoteVM.MinExpValidationAmount });
                    }

                    #endregion

                    #region Comment : Here SCENARIO-4. Multi-State selection by user
                    #endregion

                    #region Comment : Here SCENARIO-5. Companion Minimum % validation failed

                    //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                    if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 5)
                    {
                        #region Comment : Here becuase we have maintained the MinimumPayrollThreshold % for primary/companion classes

                        //Comment : Here ReferralQuote interface refernce to do/make process all business logic
                        ICaptureQuote captureQuoteBLL = new CaptureQuote();

                        BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuoteBLL.GetMinimumPayrollThreshold(appSession.StateAbbr, sessionQuoteVM.ClassDescriptionId.Value);

                        //if primaryClass data found
                        if (!primaryClassCodeData.IsNull())
                        {
                            //Comment : Here calculate user entered payroll %
                            var userEnteredPayrollPercentage = Math.Round
                                (
                                    ((Math.Round(sessionQuoteVM.AnnualPayroll, 2) / Math.Round(Convert.ToDecimal(sessionQuoteVM.TotalPayroll), 2)) * 100)
                                    , 0
                                ).ToString();

                            referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                            new
                            {
                                UserEnteredPayrollPercentage = userEnteredPayrollPercentage
                                ,
                                ClassAcceptablePayrollPercentage = Math.Round(Convert.ToDecimal(primaryClassCodeData.MinimumPayrollThreshold * 100), 0).ToString()
                            });
                        }

                        #endregion
                    }

                    #endregion

                    #region Comment : Here SCENARIO-6. FEIN/XMod flow related logics

                    appSession.QuestionnaireVM.TaxIdNumber = appSession.QuestionnaireVM.TaxIdNumber;

                    //Comment : Here in case XMod factor check all following scenarios                    
                    //If xMOD flow is not applicable
                    //If xMOD flow is applicable and a value is returned
                    //If xMOD flow is applicable and no value is returned
                    //If xMOD flow is applicable and an error message is returned

                    //Comment : Here SCENARIO-6.1 If xMOD flow is "not applicable" then 
                    if (string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber))
                    {
                        xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage1"] ?? string.Empty;
                    }
                    else
                    {
                        #region Comment : Here Scenarios where FEIN applicable

                        //Comment : Here SCENARIO-6.2 If xMOD flow is applicable and a value is returned
                        if (appSession.QuestionnaireVM.XModValue > 0)
                        {
                            xModValueMessage = Math.Round(appSession.QuestionnaireVM.XModValue, 3).ToString("0.000");
                        }
                        //Comment : Here SCENARIO-6.3 If xMOD flow is applicable and no value is returned
                        else if (referralScenarioId != 6 && appSession.QuestionnaireVM.XModValue == 0)
                        {
                            xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage2"] ?? string.Empty;
                        }
                        //Comment : Here SCENARIO-6.4 If xMOD flow is applicable and an error message is returned
                        else if (referralScenarioId == 6)
                        {
                            //same as referral-description
                            xModValueMessage = referralDescription;
                        }

                        #endregion
                    }

                    #endregion

                    #region Comment : Here SCENARIO-7. Past 3 years Claims Frequency
                    #endregion

                    #region Comment : Here SCENARIO-8. Guard rating-engine returned Referral

                    //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                    if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 8)
                    {
                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                            , new { GuardReferralMessage = sessionQuestionnaireVM.QuestionsResponse.ResultMessages });
                    }

                    #endregion

                    #region Comment : Here trace important flag/objects into log

                    //Comment : Here trace log 
                    LoggingService.Instance.Trace(
                        string.Format("ReferralScenario Found - SCENARIO: {0}, ReferralMessage : {1}, ReferralDescription : {2}"
                        , referralScenarioId
                        , (!sessionQuoteVM.IsNull() ? sessionQuestionnaireVM.QuoteReferralMessage : string.Empty)
                        , referralDescription
                        ));

                    #endregion
                }

                #endregion
            }

            #endregion

            #region Comment : Here prepare template populated with data

            var model = new ReferralQuoteMailViewModel
            {
                //Comment : Here STEP-1. Basic mail template communication information
                CompanyName = ConfigCommonKeyReader.ApplicationContactInfo["CompanyName"] ?? string.Empty,
                WebsiteUrlText = "CoverYourBusiness.com",
                WebsiteUrlHref = ConfigCommonKeyReader.SchemeAndHostURL ?? string.Empty,
                SupportEmailText = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailTextSalesSupport"] ?? string.Empty,
                SupportEmailHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailHrefSalesSupport"] ?? string.Empty,
                SupportPhoneNumber = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumber"] ?? string.Empty,
                SupportPhoneNumberHref = ConfigCommonKeyReader.ApplicationContactInfo["SupportPhoneNumberHref"] ?? string.Empty,

                //Comment : Here Additional instructions information
                ReferralAgyValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralAgyValue"] ?? string.Empty,
                ReferralBranchValue = ConfigCommonKeyReader.ApplicationContactInfo["ReferralBranchValue"] ?? string.Empty,
                ReferralLeadSourceValue = ConfigCommonKeyReader.HostURL,

                //Comment : Here STEP-2. QuoteStatus information
                ReferralStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralStatus"] ?? string.Empty,
                ReferralReason = referralReason,
                ReferralDescription = referralDescription,
                ReferralImportStatus = ConfigCommonKeyReader.ApplicationContactInfo["ReferralImportStatus"] ?? string.Empty,

                //Comment : Here STEP-3. GeneralPolicy information
                StateCode = appSession.StateAbbr,
                //logic is if "keyword-search-id" found then its search by profession
                BusinessLookupType = (!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? "Keyword" : "Dropdown",
                //logic is if It's "Keyword Search" then use BusienssName otherwise "other-class-description"
                ClassDescription = GetReferralClassDescription(sessionQuoteVM),
                //ClassDescription = 
                //(!sessionQuoteVM.ClassDescriptionKeywordId.IsNull()) ? (sessionQuoteVM.BusinessName ?? string.Empty) : (sessionQuoteVM.OtherClassDesc ?? string.Empty),
                PolicyInceptionDate = sessionQuoteVM.PolicyData.InceptionDate.Value.ToString("MM/dd/yyyy") ?? string.Empty,
                AnnualPayroll = sessionQuoteVM.TotalPayroll.ToString(),
                BusinessYears = sessionQuoteVM.BusinessYearsText,
                FeinOrSSNumber = !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber) ? appSession.QuestionnaireVM.TaxIdNumber : "NA",
                XModValueMessage = xModValueMessage,

                //Comment : Here STEP-4. Include Class information
                ClassInformationHtml = referralQuoteVM.ClassInformationHtml,

                //Comment : Here STEP-5. Include QuestionAndResponse information
                //QuestionsAndResponsesHtml = !string.IsNullOrEmpty(sbQuestionsAndResponsesHtml.ToString()) ? sbQuestionsAndResponsesHtml.ToString() : string.Empty,
                QuestionsAndResponsesHtml = referralQuoteVM.QuestionsAndResponsesHtml,

                //Comment : Here STEP-6. Referral page contact details
                QuoteId = referralQuoteVM.QuoteId,
                ContactName = referralQuoteVM.ContactName,
                BusinessName = referralQuoteVM.BusinessName,
                PhoneNumber = referralQuoteVM.PhoneNumber,
                Email = referralQuoteVM.Email,
                QuoteReferralMessage = referralQuoteVM.QuoteReferralMessage,
                LOB = LineOfBusiness,
                ZipCode = appSession.ZipCode
            };

            //Comment : Here build template and send mail
            MailTemplateBuilder buildTemplate = new MailTemplateBuilder();
            var mailMsg = buildTemplate.MailSoftReferral(model, mailContentModel);

            #endregion

            #region Comment : Here mail configuration and send mail to concern authority

            //Comment : Here assign recipeint address
            mailMsg.RecipEmailAddr = ConfigCommonKeyReader.ReferralEmailTo;
            // UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ApplicationContactInfo["SupportEmailSalesSupport"]);
            //mailMsg.Cc = UtilityFunctions.GetListOfSplitedValues(ConfigCommonKeyReader.ClientEmailID);

            mailMsg.Cc = ConfigCommonKeyReader.ReferralEmailCc;
            mailMsg.Bcc = ConfigCommonKeyReader.ReferralEmailBcc;
            mailMsg.SenderEmailAddr = ConfigCommonKeyReader.ReferralEmailFrom;

            //Send user mail
            var mailSentStatus = mailMsg != null ?
                //If attachement then accordingly
                (listOfUploadedFiles.Count > 0) ?
                    buildTemplate.SendMailWithAttachments(mailMsg, listOfUploadedFiles, true, referralQuoteVM.QuoteId.ToString()) :
                    buildTemplate.SendMail(mailMsg)
                : false;


            #endregion

            return mailSentStatus;
        }

        /// <summary>
        /// Get master of all referral reasons scenario from specified file path of XML data
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ReferralReasons GetReferralReasonsMasterFromXml(string filePath)
        {
            ReferralReasons referralReasons = null;

            try
            {
                #region Comment : Here read XML file and get all ReferralReasons

                //Comment : Here first read xml file using XmlReader helper class
                XmlFileReader xmlReader = new XmlFileReader();
                var xmlData = xmlReader.GetXmlDocumentString(filePath);

                //Comment : Here after getting XML string of ReferralReasons deserealize into refernce object for futher processing
                referralReasons = XmlParser.ToObject<ReferralReasons>(xmlData);

                if (referralReasons.IsNull())
                {
                    LoggingService.Instance.Trace("ReferralReasons master got empty");
                }

                #endregion
            }
            catch 
            {
                LoggingService.Instance.Trace("Unable to read xml & get ReferralReasons to preocess Referral");
            }

            return referralReasons.IsNull() ? new ReferralReasons() : referralReasons;
        }

        #region Private methods

        private void GetReferralReasonAndDescription(int referralScenarioId, ReferralReasons referralReasons, out string referralReason, out string referralDescription)
        {
            //Comment : Here set default value into out parameters
            referralReason = string.Empty; referralDescription = string.Empty;

            //if have scenario-id
            if (referralScenarioId > 0)
            {
                //Comment : Here filter details based on ScenrioId
                var matchedReferralReason =
                    referralReasons.ReferralReason.Where(reason => reason.ScenarioId == referralScenarioId && reason.IsActive == true).FirstOrDefault();

                //if found then
                if (!matchedReferralReason.IsNull() && matchedReferralReason.ReferralReasonText.Length > 0)
                {
                    //Comment : Here based on scenrio set Reason/Description
                    referralReason = matchedReferralReason.ReferralReasonText;
                    referralDescription = matchedReferralReason.ReferralDescription;
                }
            }
        }

        private void GetAllReferralReasons(List<int> ReferralScenarioIds, ReferralReasons referralReasons, out StringBuilder sbReferralReasons, out StringBuilder sbReferralDescriptions, out string xModValueMessage)
        {
            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;

            //local variable declaration
            string referralReason = string.Empty, referralDescription = string.Empty;

            //OUTPUT variable initialization
            sbReferralReasons = new StringBuilder(); sbReferralDescriptions = new StringBuilder();
            xModValueMessage = string.Empty;

            foreach (var referralScenarioId in ReferralScenarioIds)
            {
                //Reset while iteration
                referralReason = string.Empty; referralDescription = string.Empty;

                //Comment : Here call generic method to get specific scenario based Referral-Reason/Description
                GetReferralReasonAndDescription(referralScenarioId, referralReasons, out referralReason, out referralDescription);

                #region Comment : Here SCENARIO-General. FEIN/XMod flow related logics

                if (!appSession.QuestionnaireVM.IsNull())
                {
                    appSession.QuestionnaireVM.TaxIdNumber = appSession.QuestionnaireVM.TaxIdNumber;
                }

                //Comment : Here in case XMod factor check all following scenarios                    
                //If xMOD flow is not applicable
                //If xMOD flow is applicable and a value is returned
                //If xMOD flow is applicable and no value is returned
                //If xMOD flow is applicable and an error message is returned

                //Comment : Here SCENARIO-6.1 If xMOD flow is "not applicable" then 
                if (!appSession.QuestionnaireVM.IsNull() && string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber))
                {
                    xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage1"] ?? string.Empty;
                }
                else
                {
                    #region Comment : Here Scenarios where FEIN applicable

                    //Comment : Here SCENARIO-6.2 If xMOD flow is applicable and a value is returned
                    if (appSession.QuestionnaireVM.XModValue > 0)
                    {
                        xModValueMessage = Math.Round(appSession.QuestionnaireVM.XModValue, 3).ToString("0.000");
                    }
                    //Comment : Here SCENARIO-6.3 If xMOD flow is applicable and no value is returned
                    else if (referralScenarioId != 6 && appSession.QuestionnaireVM.XModValue == 0)
                    {
                        xModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage2"] ?? string.Empty;
                    }

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-1. Other Classification

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 1)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new { OtherClassName = string.Format("\"{0}\"", sessionQuoteVM.OtherClassDesc) });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-2. Busienss class DirectSales='E'

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 2)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { ReferralOnlyClass = sessionQuoteVM.BusinessName });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-3. Busienss class Minimum Payroll validation along with GoodState/BadState logic

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 3)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new { GoodStateAcceptableMinPayroll = sessionQuoteVM.MinExpValidationAmount });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-4. Multi-State selection by user

                //Comment : Here if it's MS scenario
                if (referralScenarioId == 4)
                {
                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-5. Companion Minimum % validation failed

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 5)
                {
                    #region Comment : Here becuase we have maintained the MinimumPayrollThreshold % for primary/companion classes

                    //Comment : Here ReferralQuote interface refernce to do/make process all business logic
                    ICaptureQuote captureQuoteBLL = new CaptureQuote();

                    BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuoteBLL.GetMinimumPayrollThreshold(appSession.StateAbbr, sessionQuoteVM.ClassDescriptionId.Value);

                    //if primaryClass data found
                    if (!primaryClassCodeData.IsNull())
                    {
                        //Comment : Here calculate user entered payroll %
                        var userEnteredPayrollPercentage = Math.Round
                            (
                                ((Math.Round(sessionQuoteVM.AnnualPayroll, 2) / Math.Round(Convert.ToDecimal(sessionQuoteVM.TotalPayroll), 2)) * 100)
                                , 0
                            ).ToString();

                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new
                        {
                            UserEnteredPayrollPercentage = userEnteredPayrollPercentage
                            ,
                            ClassAcceptablePayrollPercentage = Math.Round(Convert.ToDecimal(primaryClassCodeData.MinimumPayrollThreshold * 100), 0).ToString()
                        });
                    }

                    #endregion

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-6. FEIN/XMod flow related logics

                //Comment : Here SCENARIO-6.4 If xMOD flow is applicable and an error message is returned
                if (!appSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber) && referralScenarioId == 6)
                {
                    //same as referral-description
                    xModValueMessage = referralDescription;

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-7. Past 3 years Claims Frequency

                //Comment : Here if it's MS scenario
                if (referralScenarioId == 7)
                {
                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-8. Guard rating-engine returned Referral

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 8)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                        , new { GuardReferralMessage = sessionQuestionnaireVM.QuestionsResponse.ResultMessages });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);
                        sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-9. Guard rating-engine returned "Quote" but quote has "Referral/Decline" history

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 9)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                        , new { NA = string.Empty });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        sbReferralReasons.AppendFormat("{0} ; ", referralReason);

                        if (!string.IsNullOrEmpty(referralDescription))
                        {
                            sbReferralDescriptions.AppendFormat("{0} ; ", referralDescription);
                        }
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion
            }
        }

        /// <summary>
        /// Get list of referral reasons and descirption of supplied ReferralScenarioList
        /// </summary>
        /// <param name="referralProcessingData"></param>
        public void GetAllReferralReasonsNew(ReferralProcessing referralData)
        {
            //Comment : Here get Quote & Questionnaire session state 
            var sessionQuoteVM = appSession.QuoteVM;
            var sessionQuestionnaireVM = appSession.QuestionnaireVM;
            ReferralReasons referralReasons = GetReferralReasonsMasterFromXml(referralData.FilePath);

            //local variable declaration
            string referralReason = string.Empty, referralDescription = string.Empty;

            //OUTPUT variable initialization
            referralData.ReasonsList = new List<string>(); referralData.DescriptionList = new List<string>();
            referralData.XModValueMessage = string.Empty;

            foreach (var referralScenarioId in referralData.ReferralScenarioIds)
            {
                //Reset while iteration
                referralReason = string.Empty; referralDescription = string.Empty;

                //Comment : Here call generic method to get specific scenario based Referral-Reason/Description
                GetReferralReasonAndDescription(referralScenarioId, referralReasons, out referralReason, out referralDescription);

                #region Comment : Here SCENARIO-General. FEIN/XMod flow related logics

                if (!appSession.QuestionnaireVM.IsNull())
                {
                    appSession.QuestionnaireVM.TaxIdNumber = appSession.QuestionnaireVM.TaxIdNumber;
                }

                //Comment : Here in case XMod factor check all following scenarios                    
                //If xMOD flow is not applicable
                //If xMOD flow is applicable and a value is returned
                //If xMOD flow is applicable and no value is returned
                //If xMOD flow is applicable and an error message is returned

                //Comment : Here SCENARIO-6.1 If xMOD flow is "not applicable" then 
                if (!appSession.QuestionnaireVM.IsNull() && string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber))
                {
                    referralData.XModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage1"] ?? string.Empty;
                }
                else
                {
                    #region Comment : Here Scenarios where FEIN applicable

                    //Comment : Here SCENARIO-6.2 If xMOD flow is applicable and a value is returned
                    if (appSession.QuestionnaireVM.XModValue > 0)
                    {
                        referralData.XModValueMessage = Math.Round(appSession.QuestionnaireVM.XModValue, 3).ToString("0.000");
                    }
                    //Comment : Here SCENARIO-6.3 If xMOD flow is applicable and no value is returned
                    else if (referralScenarioId != 6 && appSession.QuestionnaireVM.XModValue == 0)
                    {
                        referralData.XModValueMessage = ConfigCommonKeyReader.ApplicationContactInfo["XModScenariosMessage2"] ?? string.Empty;
                    }

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-1. Other Classification

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 1)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new { OtherClassName = string.Format("\"{0}\"", sessionQuoteVM.OtherClassDesc) });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-2. Busienss class DirectSales='E'

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 2)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription, new { ReferralOnlyClass = sessionQuoteVM.BusinessName });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-3. Busienss class Minimum Payroll validation along with GoodState/BadState logic

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 3)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new { GoodStateAcceptableMinPayroll = sessionQuoteVM.MinExpValidationAmount.ToString("#,##0.00") });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-4. Multi-State selection by user

                //Comment : Here if it's MS scenario
                if (referralScenarioId == 4)
                {
                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-5. Companion Minimum % validation failed

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 5)
                {
                    #region Comment : Here becuase we have maintained the MinimumPayrollThreshold % for primary/companion classes

                    //Comment : Here ReferralQuote interface refernce to do/make process all business logic
                    ICaptureQuote captureQuoteBLL = new CaptureQuote();

                    BHIC.DML.WC.DTO.PrimaryClassCodeDTO primaryClassCodeData = captureQuoteBLL.GetMinimumPayrollThreshold(appSession.StateAbbr, sessionQuoteVM.ClassDescriptionId.Value);

                    //if primaryClass data found
                    if (!primaryClassCodeData.IsNull())
                    {
                        //Comment : Here calculate user entered payroll %
                        var userEnteredPayrollPercentage = Math.Round
                            (
                                ((Math.Round(sessionQuoteVM.AnnualPayroll, 2) / Math.Round(Convert.ToDecimal(sessionQuoteVM.TotalPayroll), 2)) * 100)
                                , 0
                            ).ToString();

                        referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription,
                        new
                        {
                            UserEnteredPayrollPercentage = userEnteredPayrollPercentage
                            ,
                            ClassAcceptablePayrollPercentage = Math.Round(Convert.ToDecimal(primaryClassCodeData.MinimumPayrollThreshold * 100), 0).ToString()
                        });
                    }

                    #endregion

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-6. FEIN/XMod flow related logics

                //Comment : Here SCENARIO-6.4 If xMOD flow is applicable and an error message is returned
                if (!appSession.QuestionnaireVM.IsNull() && !string.IsNullOrEmpty(appSession.QuestionnaireVM.TaxIdNumber) && referralScenarioId == 6)
                {
                    //same as referral-description
                    referralData.XModValueMessage = referralDescription;

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-7. Past 3 years Claims Frequency

                //Comment : Here if it's MS scenario
                if (referralScenarioId == 7)
                {
                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion
                }

                #endregion

                #region Comment : Here SCENARIO-8. Guard rating-engine returned Referral

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 8)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                        , new { GuardReferralMessage = sessionQuestionnaireVM.QuestionsResultMessage ?? string.Empty });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);
                        referralData.DescriptionList.Add(referralDescription);
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-9. Guard rating-engine returned "Quote" but quote has "Referral/Decline" history

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if (referralDescription.IndexOf("##") > -1 && referralScenarioId == 9)
                {
                    referralDescription = UtilityFunctions.FillTemplateWithModelValues(referralDescription
                        , new { NA = string.Empty });

                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);

                        if (!string.IsNullOrEmpty(referralDescription))
                        {
                            referralData.DescriptionList.Add(referralDescription);
                        }
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion

                #region Comment : Here SCENARIO-10,11,12,13. All DECLINE reasons

                //Comment : Here if any dynamic model value required(##objValue##) in ReferralMReason then prepare template
                if ((new List<int>() { 10, 11, 12, 13 }).Contains(referralScenarioId))
                {                    
                    #region Comment : Here "Multiple Reasons" will be added during every iteration

                    //Comment : Here club all reasons one by one
                    if (!string.IsNullOrEmpty(referralReason))
                    {
                        referralData.ReasonsList.Add(referralReason);

                        if (referralScenarioId == 13)
                        {
                            referralDescription = sessionQuestionnaireVM.QuestionsResultMessage ?? string.Empty;
                            if (!string.IsNullOrEmpty(referralDescription))
                            {
                                referralData.DescriptionList.Add(referralDescription);
                            }
                        }
                    }

                    //Comment : Here trace important flag/objects into log
                    LogReferralMessage(referralScenarioId, referralReason, referralDescription);

                    #endregion

                    //if found then no need of further below code execution
                    continue;
                }

                #endregion
            }
        }

        private string GetReferralClassDescription(QuoteViewModel sessionQuoteVM)
        {
            //Comment : Here decide final ClassDescription text based on following scenarios
            return
                //CASE-1. When user has selected "Keyword Search" then 
                    sessionQuoteVM.SelectedSearch == 0 ?
                    (sessionQuoteVM.BusinessName ?? string.Empty) :
                    (
            #region CASE-2. When user has selected "Drop-Down Selection" then

                //Comment : Here if user have gone till "ClassDescription" selection then 
                            (!sessionQuoteVM.Class.IsNull() && sessionQuoteVM.Class.ClassDescriptionId != 0) ?
                            (string.Format("{0} || {1} || {2}", sessionQuoteVM.Industry.Description, sessionQuoteVM.SubIndustry.Description, sessionQuoteVM.Class.Description)) :
                            (
                //Comment : Here if user have gone till "SubIndustry" selection only then 
                                (!sessionQuoteVM.SubIndustry.IsNull() && sessionQuoteVM.SubIndustry.SubIndustryId != 0) ?
                                string.Format("{0} || {1}", sessionQuoteVM.Industry.Description, sessionQuoteVM.SubIndustry.Description) :
                                (
                //Comment : Here if user have gone till "Industry" selection only then 
                                    (!sessionQuoteVM.Industry.IsNull() && sessionQuoteVM.Industry.IndustryId != 0) ? sessionQuoteVM.Industry.Description : string.Empty
                                )
                            )

            #endregion
);
        }

        private void LogReferralMessage(int referralScenarioId, string referralReason, string referralDescription)
        {
            #region Comment : Here trace important flag/objects into log

            //Comment : Here trace log 
            LoggingService.Instance.Trace(
                string.Format("ReferralScenario Found - SCENARIO: {0}, ReferralReason : {1}, ReferralDescription : {2}", referralScenarioId, referralReason, referralDescription));

            #endregion
        }

        #endregion

        #endregion
    }
}