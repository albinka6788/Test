using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using BHIC.Common.Client;

namespace BHIC.Contract.Policy
{
    public interface IPolicyDataService
    {
        /// <summary>
        /// Return PolicyData object based on QuoteId
        /// </summary>
        /// <returns></returns>
        PolicyDataResponse GetPolicyData(PolicyDataRequestParms args);

        /// <summary>
        /// Save new PolicyData details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPolicyData(PolicyData args);

        /// <summary>
        /// Delete existing PolicyData details based on QuoteId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeletePolicyData(PolicyDataRequestParms args);
    }
}
