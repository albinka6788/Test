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
    public class AddressService : IAddressService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of address based on QuoteId,ContactId and AddressId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Address> GetAddressList(AddressRequestParms args)
        {
            var addressResponse = SvcClientOld.CallService<AddressResponse>(string.Concat(Constants.Address,
                UtilityFunctions.CreateQueryString<AddressRequestParms>(args)));

            if (addressResponse.OperationStatus.RequestSuccessful)
            {
                return addressResponse.Addresses;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(addressResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Address details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddAddress(Address args)
        {
            var addressResponse = SvcClientOld.CallService<Address, OperationStatus>(Constants.Address, "POST", args);

            if (addressResponse.RequestSuccessful)
            {
                return addressResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(addressResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Address details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteAddress(AddressRequestParms args)
        {
            var addressResponse = SvcClientOld.CallService<AddressRequestParms, OperationStatus>(Constants.Address, "DELETE", args);

            if (addressResponse.RequestSuccessful)
            {
                return addressResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(addressResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
