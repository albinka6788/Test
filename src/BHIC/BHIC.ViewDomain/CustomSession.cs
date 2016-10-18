#region Using directives
using BHIC.Domain.Background;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;
using BHIC.Domain.QuestionEngine;
using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.QuestionEngine;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain
{
    public class CustomSession
    {
        #region Base Variables

        public string ZipCode { get; set; }
        public string StateAbbr { get; set; }
        public string PaymentConfirmationNumber { get; set; }
        public string LobId { get; set; }
        public int IsPaymentSuccess { get; set; }
        public string PaymentErrorMessage { get; set; }
        public string PaymentURL { get; set; }
        public string TransactionCode { get; set; }
        public string PolicyCentreURL { get; set; }

        public string UserEmailId { get; set; }
        public string UserPasswrod { get; set; }

        public int QuoteID { get; set; }
        public string SecureQuoteId { get; set; }
        public int PageFlag { get; set; }

        public bool IsLanding { get; set; }

        #endregion Zip Session Variables

        #region BusinessInfo/ProspactInfo Session Variables

        public BusinessInfoViewModel BusinessInfoVM { get; set; }

        #endregion

        #region Quote Session Variables

        public QuoteViewModel QuoteVM { get; set; }

        public List<CompanionClass> apiCompClassList { get; set; }

        #endregion

        #region Questionnaire Session Variables

        public QuestionnaireViewModel QuestionnaireVM { get; set; }

        #endregion

        #region QuoteSummary Session Variables

        public QuoteSummaryViewModel QuoteSummaryVM { get; set; }

        #endregion

        #region PurchaseQuote Session Variables

        public WcPurchaseViewModel PurchaseVM { get; set; }

        #endregion

        #region QuoteStatus Session Variables

        public QuoteStatusViewModel QuoteStatusVM { get; set; }

        #endregion

        public bool StateUINumberRequired { get; set; }
    }
}
