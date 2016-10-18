#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Core.Policy
{
    public class ProspectInfoService : IServiceProviders, IProspectInfoService
    {
        public ProspectInfoService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        #region Methods

        #region Interface Implementation

        /// <summary>
        /// Get the Phone(s) specified by the associated request parameters
        /// </summary>
        /// <returns></returns>
        ProspectInfoResponse IProspectInfoService.GetProspectInfo(Domain.Policy.ProspectInfoRequestParms args)
        {
            var ProspectInfoResponse = SvcClient.CallService<ProspectInfoResponse>(string.Concat("ProspectInfo",
                                        UtilityFunctions.CreateQueryString<ProspectInfoRequestParms>(args)), ServiceProvider);

            if (ProspectInfoResponse.OperationStatus.RequestSuccessful)
            {
                return ProspectInfoResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(ProspectInfoResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save the ProspetInfo specified by the request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus IProspectInfoService.AddtProspectInfo(Domain.Policy.ProspectInfo args)
        {
            var ProspectInfoResponse = SvcClient.CallService<ProspectInfo, OperationStatus>("ProspectInfo", "POST", args, ServiceProvider);

            if (ProspectInfoResponse.RequestSuccessful)
            {
                return ProspectInfoResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(ProspectInfoResponse.Messages));
            }
        }

        /// <summary>
        /// Delete the ProspectInfo specified by the request parameters
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus IProspectInfoService.DeleteProspectInfo(Domain.Policy.ProspectInfoRequestParms args)
        {
            var ProspectInfoResponse = SvcClient.CallService<ProspectInfoRequestParms, OperationStatus>("ProspectInfo", "DELETE", args, ServiceProvider);

            if (ProspectInfoResponse.RequestSuccessful)
            {
                return ProspectInfoResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(ProspectInfoResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
