#region Using Directives

using System;
using System.Collections.Generic;

using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Contract.Policy;
using BHIC.Contract.PurchasePath;
using BHIC.Core.Background;
using BHIC.Core.Policy;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.ViewDomain;

using DML_DC = BHIC.DML.WC.DataContract;
using DML_DS = BHIC.DML.WC.DataService;
using DML_DTO = BHIC.DML.WC.DTO;
using BHIC.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace BHIC.Core.PurchasePath
{
    public class QuoteSummary : IQuoteSummary
    {
        #region Variables : Class-Level local variables decalration

        protected static string LineOfBusiness = "WC";

        CustomSession appSession;
        ServiceProvider serviceProvider;

        #endregion

        #region Constructors

        public QuoteSummary(){}

        /// <summary>
        /// Initilize local instance of custom-session object to be used in different methods in this BLL
        /// </summary>
        /// <param name="customAppSession"></param>
        public QuoteSummary(CustomSession customAppSession, ServiceProvider commonServiceProvider)
        {
            appSession = customAppSession;
            serviceProvider = commonServiceProvider;
        }

        #endregion

        #region Methods : public methods

        /// <summary>
        /// Get list of applicable payment plans option for particular quote
        /// </summary>
        /// <param name="premium"></param>
        /// <returns></returns>
        public List<PaymentPlan> GetPaymentPlans(decimal premium,Int32 quoteId)
        {
            try
            {
                //Comment : Here check for application custom session object data existance
                if (appSession != null)
                {
                    #region Comment : Here local variables declaration & initialization

                    string stateCode = string.Empty;
                    stateCode = appSession.StateAbbr;

                    #endregion

                    if (stateCode.Length > 0)
                    {
                        IPaymentPlanService paymentPlanService = new PaymentPlanService(serviceProvider);
                        var paymentPlansResponse = paymentPlanService.GetPaymentPlanList(new PaymentPlanRequestParms { QuoteId = quoteId, LobAbbr = LineOfBusiness, Premium = premium, StateAbbr = stateCode });

                        return paymentPlansResponse.Count > 0 ? paymentPlansResponse : new List<PaymentPlan>();
                    }
                }
            }
            catch { }

            return new List<PaymentPlan>();
        }

        /// <summary>
        /// Get lowest installment premium amount for this Quote
        /// </summary>
        /// <returns></returns>
        public decimal GetLowestInstallmentPremium(List<PaymentPlan> paymentPlans, decimal quotePremiumAmt)
        {
            try
            {
                //Comment : Here check for application custom session object data existance
                if (appSession != null && paymentPlans != null)
                {
                    // Changed logic by Guru to handle lowest premium amount
                    decimal lowestInstallment = decimal.MaxValue;
                    foreach (PaymentPlan paymentPlan in paymentPlans)
                    {
                        decimal planInstallment = Convert.ToDecimal(((quotePremiumAmt) * paymentPlan.Down) / 100);
                        if (planInstallment < lowestInstallment)
                        {
                            lowestInstallment = planInstallment;
                        }
                    }
                    return lowestInstallment; // Convert.ToDecimal(((quotePremiumAmt) * paymentPlans[paymentPlans.Count - 1].Down) / 100);
                }
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// Get down payment or current due amount based on selected payment plan for this quote
        /// </summary>
        /// <param name="totalPremium"></param>
        /// <param name="selectedPaymentPlan"></param>
        /// <returns></returns>
        public decimal GetDownPaymentAmount(decimal totalPremium, PaymentPlan selectedPaymentPlan)
        {
            return (totalPremium * selectedPaymentPlan.Down) / 100;
        }

        /// <summary>
        /// Get FutureInstallmentAmount based on selected payment plan, total premium for this quote
        /// </summary>
        /// <param name="totalPremium"></param>
        /// <param name="selectedPaymentPlan"></param>
        /// <returns></returns>
        public decimal GetFutureInstallmentAmount(decimal totalPremium, PaymentPlan selectedPaymentPlan)
        {
            var currentDueOrDownPayment = (totalPremium * selectedPaymentPlan.Down) / 100;

            return (totalPremium - currentDueOrDownPayment) / (selectedPaymentPlan.Pays == 0 ? 1 : selectedPaymentPlan.Pays);
        }

        /// <summary>
        /// Retrun mandatory-deductible status for supplied StateCode
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public bool StateHasMandatoryDeductible(string stateCode)
        {
            try
            {
                //Comment : Here get DbConnector object
                var dataSet = GetDbConnector().LoadDataSet("GetStateMandatoryDeductible", QueryCommandType.StoredProcedure,
                    new List<System.Data.IDbDataParameter> { new SqlParameter() { ParameterName = "@StateCode", Value = stateCode, SqlDbType = SqlDbType.Char, Size = 2 } });

                if (dataSet != null && dataSet.Tables.Count>0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {
                //Comment : Here in case of DB updation failure log it into XML for future re-trying by schedular service
            }

            return false;
        }

        #region Additional methods

        /// <summary>
        /// Retrun GetQuoteSummaryVM object for quote summary page session data manipulation
        /// </summary>
        /// <returns></returns>
        public QuoteSummaryViewModel GetQuoteSummaryVM()
        {
            try
            {
                //Comment : Here check appSession object 
                if (appSession != null)
                {
                    if (appSession.QuoteSummaryVM != null)
                    {
                        return appSession.QuoteSummaryVM;
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Return interface refernce to make all database manipulation language(DML) logic
        /// </summary>
        /// <returns></returns>
        private DML_DC::IQuoteDataProvider GetQuoteProviderDML()
        {
            #region Comment : Here QuoteSummary interface refernce to do/make process all DML logic

            DML_DC::IQuoteDataProvider quoteData = new DML_DS::QuoteDataProvider();

            #endregion

            return quoteData;
        }

        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        public BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();

            #region Comment : Here using XmlReader get DB connection string

            dbConnector.DBName = "GuinnessDB";

            #endregion

            return dbConnector;
        }

        #endregion

        #region POST methods

        /// <summary>
        /// Get default selected payment plan for this QuoteId 
        /// </summary>
        /// <param name="QuoteId"></param>
        /// <returns>List<PaymentTerms></returns>
        public PaymentTerms GetPaymentTerms(int QuoteId)
        {
            IPaymentTermService paymentTermService = new PaymentTermService(serviceProvider);

            var paymentTermsResponse = paymentTermService.GetPaymentTermsList(new PaymentTermsRequestParms() { QuoteId = QuoteId });

            return paymentTermsResponse ?? new PaymentTerms();
        }

        /// <summary>
        /// Post user selected payment plan into provider system
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddPaymentTerms(PaymentTerms paymentTerms)
        {
            //Comment : Here update this payment plan into service provider system
            IPaymentTermService paymentTermService = new PaymentTermService(serviceProvider);

            var paymentTermsResponse = paymentTermService.AddPaymentTerms(paymentTerms);

            return paymentTermsResponse ?? new OperationStatus();
        }

        /// <summary>
        /// Post user new quote details into database table
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool AddUpdateQuoteData(DML_DTO::QuoteDTO quoteData)
        {
            //Comment : Here update this payment plan into service provider system
            DML_DC::IQuoteDataProvider quoteDataProvider = GetQuoteProviderDML();

            return quoteDataProvider.AddQuoteDetails(quoteData);
        }

        #endregion

        #endregion
    }
}
