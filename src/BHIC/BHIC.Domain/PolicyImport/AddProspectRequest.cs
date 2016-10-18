using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Domain.PolicyImport
{
    public class AddProspectRequest
    {
        public AddProspectRequest()
        {
            Contacts = new List<Contact>();
            Locations = new List<Location>();
            States = new List<WcState>();
            Modifiers = new List<Modifier>();
            Exposures = new List<RateClass>();
            Questions = new List<Question>();
            ScheduleModifiers = new List<SchedMod>();
            AdditionalNames = new List<AdditionalName>();
            LiabilityLimits = new LiabilityLimits();
        }

        // request credentials (ASC user account)
        public string UserName { get; set; }
        public string Password { get; set; }

        // business info

        // todo: populate name per rules from Alex 
        // briefly: 
        // - if company, put company name into name and last name; clear first and middle name )
        // - if not company, user first/middle/last name as provided by user (name is ignored; try to leave blank)
        public string Name { get; set; }					// formal business name

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string DescriptionOfOperations { get; set; }	// description of operations
        public string DBA { get; set; }						// and dba if present
        public string STTinType { get; set; }				// tin type: E or S
        public string TaxID { get; set; }
        public int YrsInBus { get; set; }					// default to zero; not currently used for the DS interface
        public string BIZType { get; set; }					// business type - select * from lookups where keyfield='BIZTYPE' and PROGRAMID='rating' order by vals

        public string Address1 { get; set; }
        public string Address2 { get; set; }				// populate with remainder of addr1 if addr1 > 25 bytes
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // contact info
        public List<Contact> Contacts { get; set; }

        // Additional InsuredNames
        public List<AdditionalName> AdditionalNames { get; set; }

        // locations - add one item to the collection, using same contact info as above
        public List<Location> Locations { get; set; }

        // distinct states from classcodes specified by the user
        public List<WcState> States { get; set; }

        // policy info
        public string POBegin { get; set; }
        public string POExpir { get; set; }
        //public string FPlanID { get; set; }	// 1/13/2015 - per Alex, replace FPlanID with FPlanEXT and Carrier (immed below)
        public string FPlanEXT { get; set; }
        public string GovState { get; set; }    // C36130-002 - CYB - WCF Policy Import Service needs to support Gov State for payment plan
        public string Carrier { get; set; }

        // exposure
        public List<RateClass> Exposures { get; set; }
        public int FullTime { get; set; }					// not really required by DS at this point; set to zero
        public int PartTime { get; set; }					// not really required by DS at this point; set to zero

        // modifiers
        public List<Modifier> Modifiers { get; set; }

        // schedule modifiers
        public List<SchedMod> ScheduleModifiers { get; set; }

        // C35807-003 - add employer liability limits
        public LiabilityLimits LiabilityLimits { get; set; }

        // questions
        public List<Question> Questions { get; set; }

        // payment info
        //public decimal PaymentAmount { get; set; }

        // misc
        public int SubmissionID { get; set; }			// not really used by Alex at this point; set to quote id for now, in the event that that becomes useful
        public string Agency { get; set; }
        //public string ProceedToIssuance { get; set; }	// Y = issue the policy

    }
}