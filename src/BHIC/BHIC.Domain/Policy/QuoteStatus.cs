using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class QuoteStatus
    {
        // ----------------------------------------
        // IDs
        // ----------------------------------------

        /// <summary>
        /// ID of the associated Quote.<br />
        /// If QuoteId is provided, the associated QuoteStatus will be updated; otherwise the QuoteStatus will be inserted, and the QuoteId will be returned in OperationStatus.AffectedIds.<br /> 
        /// </summary>
        public int? QuoteId { get; set; }

        // Per request from Jason - add a column that will be used to securely access a quote via an IIS support function.  
        // The new column will consist of an encrypted version of the following concatenated values:
        //- DsQuotes.SaveForLaterGuid
        //- DsQuotes.QuoteID
        /// <summary>
        /// Read-only Secure Quote ID.  Can be used by applications such as the IIS to securely browse to a quote hosted at the Direct Sales site (coveryourbusiness.com).
        /// </summary>
        [StringLength(100)]
        public string SecureQuoteId { get; set; }

        /// <summary>
        /// Payment Vendor's payment ID (resulting from a successful payment to a third party service such as Authorize.NET<br />
        /// </summary>
        public string PaymentTransactionId { get; set; }

        // ----------------------------------------
        // ad campaign info
        // ----------------------------------------

        /// <summary>
        /// ID associated with the ad campaign; powered by Google AdWords at the time this help text was drafted.
        /// </summary>
        [StringLength(255)]
        public string AdId { get; set; }

        // ----------------------------------------
        // user agent info
        // ----------------------------------------

        /// <summary>
        /// IP addresses of the end user accessing the Direct Sales UI.<br />
        /// Read-only.
        /// </summary>
        [StringLength(50)]
        public string UserIP { get; set; }

        /// <summary>
        /// The host of the domain currently used for the quote. This can/should be populated using the Request.Url.Host value.<br/>
        /// (examples: www.companyname.com, localhost)
        /// </summary>
        [StringLength(200)]
        public string Domain { get; set; }

        [StringLength(8)]
        public string Agency { get; set; }

        // ----------------------------------------
        // Import Status (resulting from the PolicyCreate POST request)
        // ----------------------------------------

        /// <summary>
        /// Read-only PolicyCreate POST status; text description related to RateProspect logic.  <br />
        /// </summary>
        [StringLength(50)]
        public string Import_RateProspect { get; set; }			// Values: OK, DIDNOTGETTHERE, null

        /// <summary>
        /// Read-only PolicyCreate POST status; text description related to CreateProspect logic.  <br />
        /// </summary>
        [StringLength(50)]
        public string Import_CreateProspect { get; set; }		// Values: OK, DIDNOTGETTHERE, null

        /// <summary>
        /// Read-only PolicyCreate POST status; text description related to ImportPolicy logic.  <br />
        /// </summary>
        [StringLength(50)]
        public string Import_IssuePolicy { get; set; }			// Values: OK, DIDNOTGETTHERE, null

        /// <summary>
        /// Read-only PolicyCreate POST status; text description related to ApplyPayment logic.  <br />
        /// </summary>
        [StringLength(50)]
        public string Import_ApplyPayment { get; set; }			// Values: OK, DIDNOTGETTHERE, null

        /// <summary>
        /// Read-only PolicyCreate POST status; text description related to GenerateProposal logic.  <br />
        /// </summary>
        [StringLength(50)]
        public string Import_GenerateProposal { get; set; }		// Values: OK, DIDNOTGETTHERE, null

        /// <summary>
        /// Read-only PolicyCreate POST status; text description overall import process.  <br />
        /// </summary>
        public string Import_Messages { get; set; }				// Values: OK, null, various error messsages such as ERROR: Insured Name is required. ERROR: Address is required to continue.ERROR: Invalid City (n/a), State (LA), ZIP Code (71263) combination.

        // ----------------------------------------
        // Registration Status (resulting from auto account creation, after policy is imported)
        // ----------------------------------------

        /// <summary>
        /// Optional; brief code or description associated with user registration status.  <br />
        /// </summary>
        [StringLength(10)]
        public string RegistrationStatus { get; set; }

        /// <summary>
        /// Optional; Description associated with user registration status.  <br />
        /// </summary>
        [StringLength(250)]
        public string RegistrationStatusDesc { get; set; }

        // ----------------------------------------
        // Processing Timestamps
        // ----------------------------------------

        /// <summary>
        /// WARNING: This property should only be set by the BHDQuoteCreate IIS application.<br />
        /// Time the quote was created by AAOC via the BHDQuoteCreate IIS application.  <br />
        /// This property must never be set for quotes that are entered by the customer via the CYB site.<br />
        /// </summary>
        public DateTime? CreatedByAAOC { get; set; }

        /// <summary>
        /// Time the quote was started / created.  <br />
        /// Optional; may be specified by the caller if a supplementary start time is needed by the client application, in addition to EnteredOn.<br />
        /// If not specified by the caller, defaulted to the same value as EnteredOn
        /// </summary>
        public DateTime? QuoteStarted { get; set; }

        /// <summary>
        /// Time the UI's landing page was saved.<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? LandingSaved { get; set; }

        /// <summary>
        /// Time that class(es) were selected / saved.<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? ClassesSelected { get; set; }              // C35630 - user selected / saved class(es) - added for reporting purposes

        /// <summary>
        /// Time user entered / saved exposure info for the classes selected
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? ExposuresSaved { get; set; }				// C35630 - user entered / saved exposure info for the classes selected - added for reporting purposes

        /// <summary>
        /// Timestamp associated with the successful processing the Questions POST service.  May or may not result in a quoted premium, depending on user responses provided.<br />
        /// Read-only.
        /// </summary>
        public DateTime? QuestionsSaved { get; set; }

        /// <summary>
        /// Timestamp associated with the successful processing of question responses provided by the user via the Questions POST service, resulting in a quoted premium amount.<br />
        /// Read-only.
        /// </summary>
        public DateTime? QuoteAccepted { get; set; }

        /// <summary>
        /// Timestamp associated with the successful selection of a PaymentPlanID via the PaymentTerms POST service.<br />
        /// Read-only.
        /// </summary>
        public DateTime? PaymentPlanSaved { get; set; }

        /// <summary>
        /// Timestamp associated with the use of the Contacts POST service.<br />
        /// Read-only.
        /// </summary>
        public DateTime? ContactBusinessInfoSaved { get; set; }

        /// <summary>
        /// Timestamp associated with the start of a payment request via a third party payment service. (e.g. - Via Authorize.NET).<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? SubmitToPaymentSvc { get; set; }

        /// <summary>
        /// Timestamp associated with the successful completion a payment request via a third party payment service (e.g. - Via Authorize.NET).<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? ReturnFromPaymentSvc { get; set; }

        /// <summary>
        /// Timestamp associated with the use of the PolicyCreate POST Service.<br />
        /// Read-only.
        /// </summary>
        public DateTime? PolicyIssued { get; set; }

        /// <summary>
        /// Timestamp associated with Policy Center account creation.<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? DsAccountCreated { get; set; }

        /// <summary>
        /// Associated the SavedQuotes GET service, resulting from a request from the end user to access a quote via an email link that was generated by the Save for Later link (where the link was created by the SavedQuotes POST service).<br />
        /// Read-only.
        /// </summary>
        public DateTime? SaveForLaterLoaded { get; set; }

        /// <summary>
        /// Timestamp associated with the act of loading of quote from the list of quotes within Policy Center.<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? YourQuoteLoaded { get; set; }

        /// <summary>
        /// Timestamp associated with a request by the end user to be contacted, after providing valid contact information.  (Example: the user may request to be contacted after a referral).<br />
        /// Optional; can be specified by the caller if this timestamp is useful for reporting purposes, or for UI behavior, etc... 
        /// </summary>
        public DateTime? ContactRequested { get; set; }

        /// <summary>
        /// Time the quote was started / created.  <br />
        /// Read-only.<br />
        /// See related property QuoteStarted
        /// </summary>
        public DateTime? EnteredOn { get; set; }

        /// <summary>
        /// Time that QuoteStatus data was last updated.<br />
        /// Read-only.
        /// </summary>
        public DateTime? UpdateOn { get; set; }

        // ----------------------------------------
        // BHDQuoteCreate Properties - not to be used by typical Insurance Service consumers
        // ----------------------------------------
        // Theses properties are used by the BHDQuoteCreate IIS app, which creates a Quote from an existing policy.
        // BHDQuoteCreate is used by AAOC staff for those policies that were phoned in to the AAOC, and entered directly into IIS (instead of being entered at the CYB site by the end user and imported into IIS)

        // The property below will be set to true only by the BHDQuoteCreate app, as an indicator that the Sys* properties below have been populated
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Setting this flag to true under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public bool EnableSysProperties { get; set; }

        // The property below should be populated with Insured.FplanID from the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public int SysFPlanId { get; set; }

        // The property below should be populated with WcInfo.GovState from the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public string SysGovStateAbbr { get; set; }

        // The property below should be populated with the results of Insured.PoDecPrem, for the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public decimal SysPremium { get; set; }

        // The property below should be populated with the results of fn_Bill_FeeInstall(), for the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public decimal SysPerInstallmentCharge { get; set; }

        // The property below should be populated with Insured.Carrier from the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public string SysCarrier { get; set; }

        // The property below should be populated with Insured.Agency from the policy specified by the AAOC user in the BHDQuoteCreate app
        /// <summary>
        /// WARNING: Reserved for internal Insurance Service use only.<br />
        /// This property is to be used only by internal Insurance Service logic.<br />
        /// Populating this property under normal operating conditions is undefined, and could result in unexpected behavior, including data corruption.<br />
        /// (Additional information is available in the QuoteStatus model source code.)<br />
        /// </summary>
        public string SysAgency { get; set; }
    }
}
