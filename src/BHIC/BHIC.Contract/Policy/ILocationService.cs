#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface ILocationService
    {
        /// <summary>
        /// Returns list of Location based on QuoteId and LocationId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Location> GetLocationList(LocationRequestParms args);

        /// <summary>
        /// Save new Location details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddLocation(Location args);

        /// <summary>
        /// Delete existing Location details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteLocation(LocationRequestParms args);
    }
}
