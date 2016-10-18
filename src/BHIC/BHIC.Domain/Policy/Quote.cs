using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    /// <summary>
    /// Provide access to All quote data (InsuredNames, Officers, Locations, LobData, CoverageStates, Exposures, Modifiers...
    /// </summary>
    public class Quote
    {
        // constructor
        public Quote()
        {
            // init objects to help avoid null reference exceptions
            InsuredNames = new List<InsuredName>();
            Officers = new List<Officer>();
            Locations = new List<Location>();
            LobDataList = new List<LobData>();
            Contacts = new List<Contact>();
            PolicyData = new PolicyData();
            RatingData = new RatingData();
            PaymentTerms = new PaymentTerms();
            Questions = new List<QuestionEngine.Question>();
            Documents = new List<Document>();
            QuoteStatus = new QuoteStatus();
        }

        // ----------------------------------------
        // quote data
        // ----------------------------------------

        public List<InsuredName> InsuredNames { get; set; }				// insured
        public List<Officer> Officers { get; set; }						// officers
        public List<Location> Locations { get; set; }					// mailing location, physical locations...
        public List<LobData> LobDataList { get; set; }					// coverage states / exposures / modifiers...
        public List<Contact> Contacts { get; set; }						// contact info
        public PolicyData PolicyData { get; set; }						// policy detail
        public RatingData RatingData { get; set; }						// governing state, premium, decision engine status, question engine status
        public PaymentTerms PaymentTerms { get; set; }					// payment terms associated with the payment plan selected by the user
        public List<QuestionEngine.Question> Questions { get; set; }	// question defs, associated responses, and response status (Passed validation, Hard Decline, Soft Referral, Hard Referral)
        public List<Document> Documents { get; set; }					// list of available documents

        public QuoteStatus QuoteStatus { get; set; }					// quote processing status indicators and timestamps
    }
}
