#region Using directives

using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IRatingDataService
    {
        /// <summary>
        /// Returns list of RatingData based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        RatingData GetRatingDataList(RatingDataRequestParms args);
    }
}
