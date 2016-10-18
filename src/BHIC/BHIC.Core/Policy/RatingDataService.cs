#region Using directives

using System;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;

#endregion

namespace BHIC.Core.Policy
{
    public class RatingDataService : IRatingDataService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of RatingData based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RatingData GetRatingDataList(RatingDataRequestParms args)
        {
            var ratingDataResponse = SvcClientOld.CallService<RatingDataResponse>(string.Concat(Constants.RatingData,
                UtilityFunctions.CreateQueryString<RatingDataRequestParms>(args)));

            if (ratingDataResponse.OperationStatus.RequestSuccessful)
            {
                return ratingDataResponse.RatingData;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(ratingDataResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
