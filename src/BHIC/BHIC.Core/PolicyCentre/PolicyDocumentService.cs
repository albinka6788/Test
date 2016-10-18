using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.PolicyCentre
{
    public class PolicyDocumentService : IServiceProviders, IPolicyDocumentService
    {
        public PolicyDocumentService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        /// <summary>
        /// Returns list of Policy Documents based on PolicyCode and SessionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Document> PolicyDocumentGet(PolicyDocumentRequestParms args)
        {
            var policyDocumentResponse = SvcClient.CallService<PolicyDocumentResponse>(string.Concat(Constants.PolicyDocuments,
                UtilityFunctions.CreateQueryString<PolicyDocumentRequestParms>(args)), ServiceProvider);

            if (policyDocumentResponse.OperationStatus.RequestSuccessful)
            {
                return policyDocumentResponse.PolicyDocuments;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(policyDocumentResponse.OperationStatus.Messages));
            }
        }
    }
}
