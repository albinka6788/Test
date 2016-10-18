using BHIC.Common.Client;
using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface ICityStateZipCodeSearch
    {
        /// <summary>
        /// CityStateZipCodeSearch to validate the City,State and Zipcode 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool SearchCityStateZipCode(VCityStateZipCodeRequestParms args);
    }
}
