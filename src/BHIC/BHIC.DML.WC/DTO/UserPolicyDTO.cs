using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BHIC.DML.WC.DTO
{
    /// <summary>
    /// User Policy Creation Properties
    /// </summary>
    public class UserPolicyDTO
    {
        /// <summary>
        /// Organization Name. Required, if Business Type is Corporation.
        /// </summary>
        [StringLength(200)]
        public string OrganizationName { get; set; }

        /// <summary>
        /// User Email Address, using this Email ID, user can login into Policy Center
        /// </summary>
        [StringLength(150), Required]
        [EmailAddress]
        public string EmailID { get; set; }

        /// <summary>
        /// User Login Password, used for login into Policy Center along with Email ID
        ///     In case of user(email address) exists, password will be validated whether it is matching with existing password for this user(email address). 
        ///         If password not matching, request will be terminated and return with failure response.
        ///     Password should be minimum of 8 characters!
        ///     Password must Contain at least one upper case, one lower case, one number and one special character
        /// </summary>
        [StringLength(50, ErrorMessage = "Password should be minimum of 8 characters!"),Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,50})$",
        ErrorMessage="Password must Contain at least one upper case, one lower case, one number & one special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [StringLength(255), Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [StringLength(255), Required]
        public string LastName { get; set; }
        
        /// <summary>
        /// Quote ID
        /// </summary>
        [StringLength(20), Required]
        public string QuoteNumber { get; set; }
       
        /// <summary>
        /// Total Premium Amount of the Policy
        /// </summary>
        [Required]
        public decimal TotalPremiumAmount { get; set; }

        /// <summary>
        /// Total Premium Paid for the Policy as Down Payment
        /// </summary>
        public decimal TotalPremiumPaid { get; set; }

        /// <summary>
        /// Policy Number
        /// </summary>
        [Required]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Inception Date
        /// </summary>
        [Required]
        public DateTime InceptionDate { get; set; }

        /// <summary>
        /// Next due date of the instalment
        /// </summary>
        public DateTime? NextDueDate { get; set; }
        
        /// <summary>
        /// Total No of Installments
        /// </summary>
        /// 
        [Required(ErrorMessage="No. of Installment is required")]
        [Range(0,int.MaxValue)]
        [DefaultValue(0)]
        public int Installments { get; set; }

        /// <summary>
        /// Installment Amount
        /// </summary>
        /// 
        [Required(ErrorMessage = "Installment amount is required")]
        [DefaultValue(0.0)]
        public decimal InstallmentAmount { get; set; }

        /// <summary>
        /// Installment Frequency Code. Required, If there is installments.
        /// E:	Bi-Monthly Installment
        /// A:	Annual Billing
        /// Q:	Quarterly Installments
        /// W:	Weekly Installments
        /// M:	Monthly Installments
        /// S:	Semi-Annual Installment
        /// B:	Bi-Weekly Installments
        /// N:	Semi-Monthly Installments
        /// </summary>
        public string FrequencyCode { get; set; }
    }

}
