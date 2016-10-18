using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BHIC.Domain.Policy;

namespace BHIC.ViewDomain.Landing
{
    public class WcQuoteViewModel
    {
        //public QuoteViewModel(){};

        public string Agency { get; set; }
        [Display(Name = "Business Name")]
        [Required(ErrorMessage = "Please provide the formal name of the business")]
        public string BusinessName { get; set; }
        [Display(Name = "Business Type")]
        [Required(ErrorMessage = "Please specify the type of business")]
        [StringLength(1)]
        public string BusinessType { get; set; }
        [Display(Name = "Address Line 1")]
        public string CaMailAddr1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string CaMailAddr2 { get; set; }
        [Display(Name = "City")]
        public string CaMailCity { get; set; }
        [Display(Name = "Zip")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a valid 5 digit zip code")]
        [StringLength(5, ErrorMessage = "Zip code must be  no more than 5 digits")]
        public string CaMailZip { get; set; }
        public int ClassCount { get; set; }
        [Compare("Email")]
        [Display(Name = "Confirm Email")]
        [Required(ErrorMessage = "To ensure accuracy, please confirm the email address that you provided")]
        public string ConfirmEmail { get; set; }
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "To ensure accuracy, please confirm your password")]
        public string ConfirmPassword { get; set; }
        [Compare("UserEmail")]
        [Display(Name = "Confirm Email")]
        [Required(ErrorMessage = "To ensure accuracy, please confirm the email address that you provided")]
        public string ConfirmUserEmail { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please provide your first name")]
        [StringLength(50)]
        public string ContactFirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please provide your last name.")]
        [StringLength(50)]
        public string ContactLastName { get; set; }
        [Display(Name = "Middle Name or Initial")]
        [StringLength(50)]
        public string ContactMiddleName { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Display(Name = "Contact Phone")]
        [Required(ErrorMessage = "Please provide a valid phone number")]
        public string ContactPhone { get; set; }
        [Display(Name = "DBA Name")]
        public string DbaName { get; set; }
        [Display(Name = "Description of Business")]
        [StringLength(250)]
        public string DescriptionOfOperations { get; set; }
        public decimal DownPayment { get; set; }
        public string ClassDescKeyword { get; set; }
        public int ClassDescKeywordId { get; set; }
        public int ClassDescriptionId { get; set; }
        public int IndustryId { get; set; }
        public Quote QuoteEntity { get; set; }
        [Display(Name = "Quote Reference Number")]
        public int QuoteId { get; set; }
        public int SubIndustryId { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Please provide a valid date")]
        [Display(Name = "Effective Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Remote("ValidateEffDateAdvancedView", "WcWizard", ErrorMessage = "Effective date must be a valid date no earlier than tomorrow's date, and must be within the next four months.")]
        [Required(ErrorMessage = "Please specify your policy's effective date")]
        public DateTime? EffDate { get; set; }
        [Display(Name = "Email Address")]
        [EmailAddress]
        [Required(ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }
        public decimal Exposure { get; set; }
        [Required(ErrorMessage = "Please select a payment plan")]
        public int FplanId { get; set; }
        [Display(Name = "Governing State")]
        public string GovStateAbbr { get; set; }
        public decimal LowestDownPayment { get; set; }
        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please provide an address")]
        public string MailAddr1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string MailAddr2 { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "Please specify city")]
        public string MailCity { get; set; }
        [Display(Name = "State")]
        [Required(ErrorMessage = "Please specify state")]
        public string MailState { get; set; }
        [Display(Name = "Zip")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a valid 5 digit zip code")]
        [Required(ErrorMessage = "Please provide a valid zip code")]
        [StringLength(5, ErrorMessage = "Zip code must be  no more than 5 digits")]
        public string MailZip { get; set; }
        [Display(Name = "Password")]
        //[Remote("ValidatePassword", "WcWizard", HttpMethod = "POST")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
        [Display(Name = "Due Now")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanDownAmt { get; set; }
        [Display(Name = "Future Installment Amount")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanPaymentAmt { get; set; }
        [Display(Name = "Number of Installments")]
        public int PayPlanPaymentCount { get; set; }
        public string PayPlanPaymentFreq { get; set; }
        [Display(Name = "Total Due")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayPlanTotalDue { get; set; }
        [Display(Name = "Installment Fee")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PerInstallmentCharge { get; set; }
        [Display(Name = "Annual Premium")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal? Premium { get; set; }
        [Display(Name = "Tax ID or SSN")]
        //[Remote("ValidateTIN", "WcWizard", HttpMethod = "POST")]
        [Required(ErrorMessage = "Please provide a valid SSN or Tax Id Number")]
        [StringLength(11)]
        public string TaxId { get; set; }
        [Display(Name = "Tax ID Type")]
        [Required(ErrorMessage = "Please select a valid Tax ID Type")]
        [StringLength(1)]
        public string TaxIdType { get; set; }
        [Display(Name = "Email Address")]
        public string UserEmail { get; set; }
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string UserFirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string UserLastName { get; set; }
        [Display(Name = "Contact Phone")]
        public string UserPhoneNumber { get; set; }
        //public List<WcClass_Daily> WcClass_List { get; set; }
        public string ZipCode { get; set; }
    }

}
