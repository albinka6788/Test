using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using BHIC.Domain.Background;
using BHIC.Domain.QuestionEngine;
using System.Text.RegularExpressions;

namespace BHIC.ViewDomain.Landing
{
    public class WcLandingViewModel
    {

        // ----------------------------------------
        // constructor / initialization
        // ----------------------------------------


        public WcLandingViewModel()
        {
            // initialization
            QuestionsResponse = new QuestionsResponse();
            IndustriesList = new List<Industry>();
            SubIndustriesList = new List<SubIndustry>();
            ClassDescriptionsList = new List<ClassDescription>();
            StateList = new List<State>();

            //ModifierViewModels = new List<DirectSales.BusinessLogic.DsModifierViewModel>();
            //DsClasses = new List<DsClass>();			
        }

        // ----------------------------------------
        // select and display lists
        // ----------------------------------------

        // industry select list
        public List<Industry> IndustriesList { get; set; }

        // sub-industry select list
        public List<SubIndustry> SubIndustriesList { get; set; }

        // class description select list
        public List<ClassDescription> ClassDescriptionsList { get; set; }

        // payment plan select list
        //public List<Fplans> Fplans { get; set; }


        // ----------------------------------------
        // Quote Data (persisted primary class, business, contact, etc... parameters provided by user)
        // ----------------------------------------

        public WcQuoteViewModel QuoteViewModel { get; set; }

        // ----------------------------------------
        // select lists
        // ----------------------------------------

        public List<State> StateList { get; set; }

        // ----------------------------------------
        // questions and responses
        // ----------------------------------------		

        // questions from the engine data /  responses provided by the user
        public QuestionsResponse QuestionsResponse { get; set; }

        // ----------------------------------------
        // data provided by the user
        // ----------------------------------------

        // ---------- If it is populated --------
        public bool IsPopulated { get; set; }

        // ---------- Quote Parameters ---------- 
        [Required(ErrorMessage = "Please provide a valid zip code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }

        public DateTime MaximumDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        [DataType(DataType.Date, ErrorMessage = "Please provide a valid date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Effective Date")]
        [Required(ErrorMessage = "Please specify your policy's Start Date")]
        //[Remote("ValidateEffDate", "WcWizard", HttpMethod="POST")]
        public DateTime? EffDate { get; set; }

        [Required(ErrorMessage = "Please select an Industry")]
        [Display(Name = "Industry")]
        public int DsIndustryId { get; set; }

        [Required(ErrorMessage = "Please select a Sub-Industry")]
        [Display(Name = "Type of Job")]
        public int DsSubIndustryId { get; set; }

        [Required(ErrorMessage = "Please select a Business")]
        [Display(Name = "Business")]
        public int DsClassDescriptionId { get; set; }

        [Required(ErrorMessage = "Please enter a Business")]
        [Display(Name = "Business")]
        public int? DsClassDescKeywordId { get; set; }

        [Required(ErrorMessage = "Please enter a Business")]
        public string DsClassDescKeyword { get; set; }

        // flag that indicates whether or not the user is searching by keyword (false, if searching by Industry/Subindustry/ClassDesc)
        public bool DsClassDescKeywordSearchSelected { get; set; }

        // class description text entered by user on the landing page (when there's not an appropriate class associated with the selected industry and subindustry)
        [StringLength(50)]
        [Required(ErrorMessage = "Please provide a Business Description")]
        //[Remote("ValidateClassText", "WcWizard", ErrorMessage = "Please provide a Business Description", AdditionalFields = "DsClassDescriptionId", HttpMethod = "POST")]
        public string ClassText { get; set; }

        // annual payroll
        public decimal? Exposure { get; set; }

        // NOTE: using a string to capture exposure from the UI, so it can contain commas, etc... without failing validation
        // the value provided by the user is set int the Exposure property above
        [Required(ErrorMessage = "Please provide Annual Employee Payroll")]
        [Display(Name = "Annual Payroll")]
        //[Remote("ValidateMinExposure", "WcWizard", ErrorMessage = "Invalid payroll amount", AdditionalFields = "DsClassDescriptionId", HttpMethod="POST")]
        public string ExposureString
        {
            get { return (Exposure != null && Exposure != 0) ? this.Exposure.ToString() : ""; }
            set
            {
                // strip everything but digits and decimal points from the string
                if (value != null)
                {
                    this.Exposure = decimal.Parse(Regex.Replace(value, @"[^-?\d+\.]", ""));
                }
            }
        }

        // plan id
        public int FplanId { get; set; }

        // ---------- Contact Information ---------- 
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Security Code")]
        public string CCsecurityCode { get; set; }

        // ----------------------------------------
        // other derived and calc'd data, ui state, etc...
        // ----------------------------------------

        public string StateAbbr { get; set; }
        public string StateName { get; set; }
        public string DsUniqueVisitorId { get; set; }		// identifies a unique unauthenticated user

        [Display(Name = "Due Now")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanDownAmt { get; set; }

        public string PayPlanPaymentFreq { get; set; }

        [Display(Name = "Future Installment Amount")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanPaymentAmt { get; set; }

        [Display(Name = "Number of Installments")]
        public int PayPlanPaymentCount { get; set; }

        [Display(Name = "Total Due")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanTotalDue { get; set; }


        // placeholder for page-level msgs
        public string DsMessageSummary { get; set; }

        // if the user accepted the accord question (the last question on the page), this value will be true
        // controls navigation elements in the UI that prevent the user from proceeding if this hasn't been checked yet
        public bool DsACORDAcceptChecked { get; set; }

    }

    public class WcHomePageViewModel
    {
        // from spec: Payclass/ClassDescriptionID, Exposure/Payroll {ie. 0953 or ClassDescId=172; $400,000 or 5 Firetrucks }

        // ----------------------------------------
        // Direct Sales Class Info
        // ----------------------------------------

        /// <summary>
        /// ID of the Exposure associated with the operation.<br />
        /// If provided, specifies the ID of the Exposure to update, otherwise the Exposure will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? ExposureId { get; set; }

        /// <summary>
        /// ID of the Quote that the exposure is assigned to.<br />
        /// Initially obtained by calling the QuoteState POST service.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// </summary>
        [Required]
        public int? QuoteId { get; set; }

        /// <summary>
        /// ID of the Industry assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.  Required for a Simple flow.  Not relevant for Advanced flow.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) If SimpleFlow = true, one of the following cases must be satisfied:<br />
        ///    - A valid IndustryId / SubIndustryId / ClassIndustry selection must be provided.<br />
        ///      or<br />
        ///    - A valid ClassDescriptionKeywordId must be provided.<br />
        /// </summary>
        public int? IndustryId { get; set; }

        /// <summary>
        /// ID of the SubIndustry assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.  Required for a Simple flow.  Not relevant for Advanced flow.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) If SimpleFlow = true, one of the following cases must be satisfied:<br />
        ///    - A valid IndustryId / SubIndustryId / ClassIndustry selection must be provided.<br />
        ///      or<br />
        ///    - A valid ClassDescriptionKeywordId must be provided.<br />
        /// </summary>
        public int? SubIndustryId { get; set; }

        /// <summary>
        /// ID of the ClassDescription assigned to the Exposure. Associated with use of Industry/Subindustry/ClassDescription for class selection.  Required for a Simple flow.  Not relevant for Advanced flow.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) If SimpleFlow = true, one of the following cases must be satisfied:<br />
        ///    - A valid IndustryId / SubIndustryId / ClassIndustry selection must be provided.<br />
        ///      or<br />
        ///    - A valid ClassDescriptionKeywordId must be provided.<br />
        /// </summary>
        public int? ClassDescriptionId { get; set; }

        /// <summary>
        /// ID of the ClassDescriptionKeyword assigned to the Exposure. Associated with use of Keyword Search for class selection.  Required for a Simple flow.  Not relevant for Advanced flow.<br />
        /// Example: 123<br />
        /// Validation:<br />
        /// 1) Must be a valid existing value.<br />
        /// </summary>
        public int? ClassDescriptionKeywordId { get; set; }

        /// <summary>
        /// 5 digit Zip code associated with the Exposure.  Required for a Simple flow.  Not relevant for Advanced flow.<br />
        /// Example: 71263<br />
        /// Validation:<br />
        /// 1) Either StateAbbr or ZipCode or both may be provided when submitting an exposure.<br />
        /// 2) If only state is provided (e.g. - advanced flow), only state is persisted.<br />
        /// 3) If only zip code is provided (e.g. - simple flow), state will be derived from zip code, and both will be persisted.<br />
        /// 4) When both are persisted, the zip/state combination will be validated.<br />
        /// 5) Zip code length must be 5 characters.<br />
        /// 6) State abbreviation must be 2 characters.<br />
        /// </summary>
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Set to true if the Exposure is being added via the simple flow, otherwise false. (Required because the flows use different methods to collect exposure data; state vs zip, etc...)<br />
        /// Example: true<br />
        /// Validation:<br />
        /// 1) Value must not be null.<br />
        /// </summary>
        [Required]
        public bool? SimpleFlow { get; set; }

        // ----------------------------------------
        // Standard GUARD Class Info
        // ----------------------------------------

        /// <summary>
        /// ClassCode associated with the exposure.  Provided by the user from their existing WC policy documentation.  Required for a Advanced flow.  Not relevant for Simple flow.<br />
        /// Example: 8803<br />
        /// Validation:<br />
        /// 1) Required only if SimpleFlow != true<br />
        /// 2) Must be a valid existing value.<br />
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// ClassSuffix associated with the exposure.  Taken from the Class Description selected by the user during the Advanced flow.  Not relevant for Simple flow.<br />
        /// Example: 01<br />
        /// Validation:<br />
        /// 1) Required only if SimpleFlow != true<br />
        /// 2) Must be a valid existing value.<br />
        /// </summary>
        public string ClassSuffix { get; set; }

        /// <summary>
        /// Annual Payroll associated with the exposure.  Required.<br />
        /// Example: 150000<br />
        /// Validation:<br />
        /// 1) Required.<br />
        /// 2) Amount specified must be >= the configurable minimum amount established for the class / state.<br />
        /// </summary>
        [Required(ErrorMessage = "The annual premium amount is required.")]
        public string ExposureAmt { get; set; }

        /// <summary>
        /// State associated with the exposure.<br />
        /// Example: NY<br />
        /// Validation:<br />
        /// 1) Either StateAbbr or ZipCode or both may be provided when submitting an exposure.<br />
        /// 2) If only state is provided (e.g. - advanced flow), only state is persisted.<br />
        /// 3) If only zip code is provided (e.g. - simple flow), state will be derived from zip code, and both will be persisted.<br />
        /// 4) When both are persisted, the zip/state combination will be validated.<br />
        /// 5) Zip code length must be 5 characters.<br />
        /// 6) State abbreviation must be 2 characters.<br />
        /// </summary>
        public string StateAbbr { get; set; }

        public string InceptionDate { get; set; }

        public int? BusinessYears { get; set; }

        public bool IsGoodStateApplicable { get; set; }

        public bool IsMultiClassApplicable { get; set; }

        public bool IsMultiStateApplicable { get; set; }

        public bool IsMultiClass { get; set; }

        public string TotalPayroll { get; set; }

        public bool MoreClassRequired { get; set; }

        public decimal MinExpValidationAmount { get; set; }

        public List<CompanionClassData> CompClassData { get; set; }

        [StringLength(50)]
        public string OtherClassDesc { get; set; }

        //For BusinessClass DirectSales value
        public string BusinessClassDirectSales { get; set; }

        public string BusinessName { get; set; }

        public string IndustryName { get; set; }

        public string SubIndustryName { get; set; }

        public string BusinessYearsText { get; set; }
    }

    public class CompanionClassData
    {
        /// <summary>
        /// ID of the CompanionClass.<br />
        /// Example: 123<br />
        /// </summary>
        public int? CompanionClassId { get; set; }

        /// <summary>
        /// User-friendly name for the CompanionClass.<br />
        /// Example: Clerical<br />
        /// </summary>
        public string FriendlyLabel { get; set; }

        /// <summary>
        /// ClassCode associated with the ClassDescription
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// ClassSuffix associated with the ClassClassDescription
        /// </summary>
        public string ClassSuffix { get; set; }

        /// <summary>
        /// HelpText associcated with companion class
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Payroll Amount related to each comapnion class
        /// </summary>
        public string PayrollAmount { get; set; }

        /// <summary>
        /// ID of the Exposure associated with the operation.<br />
        /// If provided, specifies the ID of the Exposure to update, otherwise the Exposure will be inserted, and the ID returned in OperationStatus.AffectedIds.<br /> 
        /// Example: 123<br />
        /// </summary>
        public int? ExposureId { get; set; }

        /// <summary>
        /// This field will capture whether exposure amo
        /// </summary>
        public bool? IsExposureAmountRequired { get; set; }

        /// <summary>
        /// Question to be displayed
        /// </summary>
        [StringLength(2000)]
        public string PromptText { get; set; }
    }

}
