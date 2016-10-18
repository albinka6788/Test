#region Using directives

using BHIC.Common.Client;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IPolicyCreateService
    {
        /// <summary>
        /// Create new policy
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPolicy(PolicyCreate args);
    }
}
