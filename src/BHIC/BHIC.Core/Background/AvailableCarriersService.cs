using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Contract.Background;
using BHIC.Domain.Background;
using BHIC.Common.Client;
using BHIC.Common;
using BHIC.Common.Config;
using BHIC.Contract.Provider;

namespace BHIC.Core.Background
{
    public class AvailableCarriersService : IServiceProviders,IAvailableCarriersService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Available Carriers based on EffectiveDate, LOB
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableCarriersList(AvailableCarriersRequestParms args)
        {
            var availableCarriersResponse = SvcClient.CallService<AvailableCarriersResponse>(
                string.Concat(Constants.AvailableCarriers, UtilityFunctions.CreateQueryString<AvailableCarriersRequestParms>(args)),ServiceProvider);

            if (availableCarriersResponse.OperationStatus.RequestSuccessful)
            {
                return availableCarriersResponse.Carriers;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(availableCarriersResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
