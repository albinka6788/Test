using BHIC.ViewDomain.Mailing;

namespace BHIC.ViewDomain
{
    public class ReferralQuoteMailViewModel : MailTemplatesBaseViewModel
    {
        #region Variables : Page Level Local Variables Decalration

        public string ContactName { get; set; }
        public string BusinessName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        //Comment : Here Additional instructions information
        public string ReferralAgyValue { get; set; }
        public string ReferralBranchValue { get; set; }
        public string ReferralLeadSourceValue { get; set; }

        //Comment : Here QuoteStatus information
        public string QuoteReferralMessage { get; set; }
        public string ReferralStatus { get; set; }
        public string ReferralReason { get; set; }
        public string DeclineReason { get; set; }
        public string ReferralDescription { get; set; }
        public string ReferralImportStatus { get; set; }

        //Comment : Here GeneralPolicy information
        public string StateCode           { get; set; }
        public string BusinessLookupType  { get; set; }
        public string ClassDescription    { get; set; }
        public string PolicyInceptionDate { get; set; }
        public string AnnualPayroll { get; set; }
        public string BusinessYears { get; set; }
        public string FeinOrSSNumber { get; set; }
        public string XModValueMessage { get; set; }
        public string ZipCode { get; set; }
        public string EstimatedTotalPremium { get; set; }

        //Comment : Here Class information 
        public string ClassInformationHtml { get; set; }
        public string QuestionsAndResponsesHtml { get; set; }

        #endregion
    }
}
