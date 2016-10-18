#region Using directives

using System;
using System.Collections.Generic;

using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Common.Client;
using BHIC.Contract.Provider;

#endregion

namespace BHIC.Core.Background
{
    public class PaymentPlanService : IServiceProviders,IPaymentPlanService
    {

        #region Constructors

        public PaymentPlanService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of payment plans based on LobAbbr,StateAbbr,Premium and PaymentPlanId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<PaymentPlan> IPaymentPlanService.GetPaymentPlanList(PaymentPlanRequestParms args)
        {
            var paymentResponse = SvcClient.CallService<PaymentPlanResponse>(string.Concat(Constants.PaymentPlans,
                UtilityFunctions.CreateQueryString<PaymentPlanRequestParms>(args)), ServiceProvider);

            if (paymentResponse.OperationStatus.RequestSuccessful)
            {
                return paymentResponse.PaymentPlans;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(paymentResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
