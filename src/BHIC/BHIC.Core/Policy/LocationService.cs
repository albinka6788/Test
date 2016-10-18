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
    public class LocationService : ILocationService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Location based on QuoteId and LocationId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Location> GetLocationList(LocationRequestParms args)
        {
            var locationResponse = SvcClientOld.CallService<LocationResponse>(string.Concat(Constants.Location,
                UtilityFunctions.CreateQueryString<LocationRequestParms>(args)));

            if (locationResponse.OperationStatus.RequestSuccessful)
            {
                return locationResponse.Locations;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(locationResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Location details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddLocation(Location args)
        {
            var locationResponse = SvcClientOld.CallService<Location, OperationStatus>(Constants.Location, "POST", args);

            if (locationResponse.RequestSuccessful)
            {
                return locationResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(locationResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Location details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteLocation(LocationRequestParms args)
        {
            var locationResponse = SvcClientOld.CallService<LocationRequestParms, OperationStatus>(Constants.Location, "DELETE", args);

            if (locationResponse.RequestSuccessful)
            {
                return locationResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(locationResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
