#region Using directives

using BHIC.Domain.Background;
using System;

#endregion

namespace BHIC.Contract.Background
{
    public interface IVCityStateZipCodeService
    {
         /// <summary>
        /// Return VCityStateZipCode object based on City,State and Zip
        /// </summary>
        /// <returns></returns>
        VCityStateZipCodeResponse GetVCityStateZipCodeData(VCityStateZipCodeRequestParms args);
    }
}
