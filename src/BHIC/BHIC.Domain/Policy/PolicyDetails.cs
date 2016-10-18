using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class PolicyDetails
    {
        // constructor
        public PolicyDetails()
        {
            Agency = new Agency();
            Insured = new Insured();
            RenewDetails = new RenewDetails();
            BackEndContacts = new List<BackEnd.Contact>(0);
        }

        public Agency Agency { get; set; }
        public Insured Insured { get; set; }
        public RenewDetails RenewDetails { get; set; }

        [StringLength(10)]
        public string PolicyCode { get; set; }

        public DateTime PolicyBegin { get; set; }
        public DateTime PolicyExpires { get; set; }

        [StringLength(2)]
        public string GoverningState { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
        public bool IsActive { get; set; }

        //Added by CUB team
        public string CYBPolicyNumber { get; set; }
        public int LOB { get; set; }

        // C36040 - add back-end contact information
        /// <summary>
        /// Contact information from back-end insurance system.
        /// </summary>
        public List<BackEnd.Contact> BackEndContacts { get; set; }
    }
}