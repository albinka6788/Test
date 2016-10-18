using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Policy;
using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;

namespace BHIC.Core.PolicyCentre
{
    public class PolicyCancellation : IPolicyCancellation
    {
        /// <summary>
        /// Returns list of Policy Documents based on PolicyCode and SessionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<CancellationRequest> GetCancellationDetails(CancellationRequestParms args, ServiceProvider provider)
        {
            var cancellationRequestResponse = SvcClient.CallService<CancellationRequestResponse>(string.Concat(Constants.CancellationPolicy,
                UtilityFunctions.CreateQueryString<CancellationRequestParms>(args)), provider);

            if (cancellationRequestResponse.OperationStatus.RequestSuccessful)
            {
                return cancellationRequestResponse.CancellationRequests;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(cancellationRequestResponse.OperationStatus.Messages));
            }
        }
    }
}
