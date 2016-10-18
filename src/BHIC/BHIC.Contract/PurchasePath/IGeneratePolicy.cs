#region Using directives

using System;

using BHIC.Common.Client;

#endregion

namespace BHIC.Contract.PurchasePath
{
    public interface IGeneratePolicy
    {
        /// <summary>
        /// It will create new policy using PolicyCreate api
        /// </summary>
        /// <param name="lob">Line of business</param>
        /// <param name="quoteId">Quote Id</param>
        /// <param name="serviceProvider">Service Provider Name</param>
        /// <returns>Return true if successfully created, false otherwise</returns>
        string CreatePolicy(string lob, string userEmail, int quoteId, ServiceProvider serviceProvider);
    }
}
