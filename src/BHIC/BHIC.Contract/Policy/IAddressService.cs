#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IAddressService
    {
        /// <summary>
        /// Returns list of address based on QuoteId,ContactId and AddressId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Address> GetAddressList(AddressRequestParms args);

        /// <summary>
        /// Save new Address details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddAddress(Address args);

        /// <summary>
        /// Delete existing Address details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteAddress(AddressRequestParms args);
    }
}
