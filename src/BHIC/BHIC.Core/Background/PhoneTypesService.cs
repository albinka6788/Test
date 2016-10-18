using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract;
using BHIC.Domain.Background;
using BHIC.Contract.Provider;
using BHIC.Common;
using BHIC.Common.Client;

namespace BHIC.Core.Background
{
    public class PhoneTypesService : IServiceProviders, IPhoneTypesService
    {
        public PhoneTypesService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        /// <summary>
        /// Get phone Types based on request
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<PhoneType> GetPhoneTypes(PhoneTypeRequestParms args)
        {
            var phoneTypesResponse = SvcClient.CallService<PhoneTypeResponse>(string.Concat("PhoneTypes",
                UtilityFunctions.CreateQueryString<PhoneTypeRequestParms>(args)), ServiceProvider);

            if (phoneTypesResponse.OperationStatus.RequestSuccessful)
            {
                return phoneTypesResponse.PhoneTypes;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(phoneTypesResponse.OperationStatus.Messages));
            }
        }
    }
}
