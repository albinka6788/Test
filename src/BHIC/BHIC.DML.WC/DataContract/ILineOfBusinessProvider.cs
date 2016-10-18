#region Using directives

using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface ILineOfBusinessProvider
    {
        /// <summary>
        /// Fetch list fo available line of business from database
        /// </summary>
        /// <returns>returns list of all lob</returns>
        List<LineOfBusiness> GetAllLineOfBusiness();
    }
}
