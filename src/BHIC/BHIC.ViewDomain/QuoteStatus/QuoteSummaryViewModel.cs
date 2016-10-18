using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.ViewDomain.QuestionEngine;
using BHIC.ViewDomain.Landing;

namespace BHIC.ViewDomain
{
    public class QuoteSummaryViewModel : QuestionnaireViewModel
    {
        #region Comment : Here constructor / initialization

        public QuoteSummaryViewModel()
        {
            // init objects to help avoid issues related to null reference exceptions
            PaymentPlans = new List<PaymentPlan>();
            PaymentTerms = new PaymentTerms();
            SelectedPaymentPlan = new PaymentPlan();

            QuoteReferenceNo = string.Empty;
            LowestInstallmentPremium = 0;
            HasMandatoryDeductible = false;

            NavigationLinks = new List<NavigationModel>();

            deductibleModiferId = null;
            limitModiferId = null;
            stateName = string.Empty;
            Deductibles = new List<WCDeductibles>();
            selectedDeductible = new WCDeductibles();
            ListOfDeductiblesTypes = new List<WCDeductibleType>();
            employeeLimitText = string.Empty;
            employeeLimitValue = string.Empty;
            policyLimitValue = 0;
            btnText = string.Empty;
            saveForLaterFlag = false;
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public List<PaymentPlan> PaymentPlans { get; set; }
        public PaymentTerms PaymentTerms { get; set; }

        public string QuoteReferenceNo { get; set; }
        public decimal LowestInstallmentPremium { get; set; }

        public PaymentPlan SelectedPaymentPlan { get; set; }
        public bool HasMandatoryDeductible { get; set; }

        public List<NavigationModel> NavigationLinks { get; set; }

        public int? deductibleModiferId { get; set; }
        public int? limitModiferId { get; set; }
        public string stateName { get; set; }
        public List<WCDeductibles> Deductibles { get; set; }
        public WCDeductibles selectedDeductible { get; set; }
        public List<WCDeductibleType> ListOfDeductiblesTypes { get; set; }
        public string employeeLimitText { get; set; }
        public string employeeLimitValue { get; set; }
        public decimal policyLimitValue { get; set; }
        public string btnText { get; set; }
        public bool saveForLaterFlag { get; set; }

        #endregion
    }
}
