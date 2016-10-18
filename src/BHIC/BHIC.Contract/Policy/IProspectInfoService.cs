#region Using dirctives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IProspectInfoService
    {
        /// <summary>
        /// Get the Phone(s) specified by the associated request parameters
        /// </summary>
        /// <returns></returns>
        ProspectInfoResponse GetProspectInfo(ProspectInfoRequestParms args);

        /// <summary>
        /// Save the ProspetInfo specified by the request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddtProspectInfo(ProspectInfo args);

        /// <summary>
        /// Delete the ProspectInfo specified by the request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteProspectInfo(ProspectInfoRequestParms args);
    }
}
