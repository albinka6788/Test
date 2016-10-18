using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.Provider;
using BHIC.Domain.Background;
namespace BHIC.Core.PolicyCentre
{
    public class CityStateZipCodeSearch : IServiceProviders, ICityStateZipCodeSearch
    {
        public CityStateZipCodeSearch(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        /// <summary>
        /// View Documents based on DocumentId, PolicyCode and SessionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SearchCityStateZipCode(VCityStateZipCodeRequestParms args)
        {
            bool result = false;
            var cityStateZipCodeResponse = SvcClient.CallService<VCityStateZipCodeResponse>(string.Concat(Constants.CityStateZipcode,
                UtilityFunctions.CreateQueryString<VCityStateZipCodeRequestParms>(args)), ServiceProvider);

            if (cityStateZipCodeResponse.OperationStatus.RequestSuccessful)
            {
                result = true;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(cityStateZipCodeResponse.OperationStatus.Messages));
            }
            return result;
        }
    }
}
