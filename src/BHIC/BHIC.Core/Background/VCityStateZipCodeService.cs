#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Background;
using BHIC.Contract.Provider;
using BHIC.Domain.Background;
using System;

#endregion

namespace BHIC.Core.Background
{
    public class VCityStateZipCodeService : IServiceProviders,IVCityStateZipCodeService 
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Return VCityStateZipCode object based on City,State and Zip
        /// </summary>
        /// <returns></returns>
        VCityStateZipCodeResponse IVCityStateZipCodeService.GetVCityStateZipCodeData(VCityStateZipCodeRequestParms args)
        {
            var ZipCityStateResponse = SvcClient.CallService<VCityStateZipCodeResponse>(string.Concat("VCityStateZipCode",
                                           UtilityFunctions.CreateQueryString<VCityStateZipCodeRequestParms>(args)),ServiceProvider);

            if (ZipCityStateResponse.OperationStatus.RequestSuccessful)
            {
                return ZipCityStateResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(ZipCityStateResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
