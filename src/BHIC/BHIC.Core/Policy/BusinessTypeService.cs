using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.Policy;
using BHIC.Domain.Background;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Common;
using BHIC.Contract.Provider;
namespace BHIC.Core.Policy
{
    public class BusinessTypeService : IServiceProviders, IBusinessTypeService
    {
        /// <summary>
        /// Get Business Types
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<BusinessType> GetBusinessTypes(BusinessTypeRequestParms args)
        {
            var businessTypeResponse = SvcClient.CallService<BusinessTypeResponse>(string.Concat(Constants.ServiceNames.BusinessTypes,
                   UtilityFunctions.CreateQueryString<BusinessTypeRequestParms>(args)), ServiceProvider);

            if (businessTypeResponse.OperationStatus.RequestSuccessful)
            {
                return businessTypeResponse.BusinessTypes;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(businessTypeResponse.OperationStatus.Messages));
            }
        }
    }
}
