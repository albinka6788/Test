#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class OfficerService : IOfficerService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Officer based on QuoteId and OfficerId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Officer> GetOfficerList(OfficerRequestParms args)
        {
            var officerResponse = SvcClientOld.CallService<OfficerResponse>(string.Concat(Constants.Officer,
                UtilityFunctions.CreateQueryString<OfficerRequestParms>(args)));

            if (officerResponse.OperationStatus.RequestSuccessful)
            {
                return officerResponse.Officers;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(officerResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Officer details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddOfficer(Officer args)
        {
            var officerResponse = SvcClientOld.CallService<Officer, OperationStatus>(Constants.Officer, "POST", args);

            if (officerResponse.RequestSuccessful)
            {
                return officerResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(officerResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Officer details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteOfficer(OfficerRequestParms args)
        {
            var officerResponse = SvcClientOld.CallService<OfficerRequestParms, OperationStatus>(Constants.Officer, "DELETE", args);

            if (officerResponse.RequestSuccessful)
            {
                return officerResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(officerResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
