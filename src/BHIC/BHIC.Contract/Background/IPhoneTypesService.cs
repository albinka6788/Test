using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract
{
    public interface IPhoneTypesService
    {
        /// <summary>
        /// Get phone types 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<PhoneType> GetPhoneTypes(PhoneTypeRequestParms args);
    }
}
