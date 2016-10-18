using BHIC.Domain.QuestionEngine;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;

namespace BHIC.ViewDomain.QuestionEngine
{
    public class QuestionnaireViewModel
    {
        #region Comment : Here constructor / initialization

        public QuestionnaireViewModel()
        {
            // initialize Questions lookups
            QuestionsResponse = new QuestionsResponse();
            TaxIdNumber = string.Empty;
            XModValue = 0;
            InstallmentFee = 0;
            PremiumAmt = 0;
            QuoteStatus = string.Empty;
            Agency = string.Empty;
            Carrier = string.Empty;
            XModFactorModifierId = null;
            QuoteReferralMessage = string.Empty;
            ReferralScenarioId = 0;
            QuoteReferralMessages = new List<string>();
            ReferralScenarioIds = new List<int>();
            ReferralScenariosHistory = new List<List<int>>();
            ReferralHistory = new ReferralHistory();
            QuestionsResultMessage = string.Empty;
            FeinApplicable = false;
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public QuestionsResponse QuestionsResponse { get; set; }
        public string TaxIdNumber { get; set; }
        public string TaxIdType { get; set; }
        public decimal XModValue { get; set; }
        public DateTime? XModExpiryDate { get; set; }
        public decimal InstallmentFee { get; set; }
        public decimal PremiumAmt { get; set; }
        public string QuoteStatus { get; set; }
        public string Agency { get; set; }
        public string Carrier { get; set; }
        public int? XModFactorModifierId { get; set; }
        public string QuoteReferralMessage { get; set; }
        public int ReferralScenarioId { get; set; }
        //For new implementation (Mutiple Reasons Tracking Across PurchasePath)
        public List<string> QuoteReferralMessages { get; set; }
        public List<int> ReferralScenarioIds { get; set; }
        //For new implementation on 28.04.2016 (Complete History Along With Set Of Mutiple Reasons Tracking Across PurchasePath)
        public List<List<int>> ReferralScenariosHistory { get; set; }
        //Comment : Here [GUIN-270-Prem] For new implementation of on 01.06.2016 (We have to keep all Referral Reasons along with thier running dynamic values during Referral tracking across PurchasePath)
        public ReferralHistory ReferralHistory { get; set; }
        public string QuestionsResultMessage { get; set; }
        public bool FeinApplicable { get; set; }

        #endregion

    }

    public class QuestionnaireViewModelAngular
    {
        #region Comment : Here constructor / initialization

        public QuestionnaireViewModelAngular()
        {
            // initialize Questions lookups
            Questions = new List<Question>();
            FeinApplicable = false;
            TaxIdNumber = string.Empty;
            TaxIdType = string.Empty;
            Messages = new List<string>();
            NavigationLinks = new List<NavigationModel>();
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public List<Question> Questions { get; set; }
        public bool FeinApplicable { get; set; }
        public string TaxIdNumber { get; set; }
        public string TaxIdType { get; set; }
        public List<string> Messages { get; set; }
        public List<NavigationModel> NavigationLinks { get; set; }


        #endregion

    }
}
