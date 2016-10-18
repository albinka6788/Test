#region Using directives

using BHIC.DML.WC.DTO;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.DML.WC.DataContract
{
    public interface IMultipleStateDataProvider
    {
        List<ZipCodeStates> GetStatesByZipCode();
    }
}
