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
    public class PhoneService : IPhoneService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Phone based on QuoteId,ContactId and PhoneId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Phone> GetPhoneList(PhoneRequestParms args)
        {
            var phoneResponse = SvcClientOld.CallService<PhoneResponse>(string.Concat(Constants.Phone,
                UtilityFunctions.CreateQueryString<PhoneRequestParms>(args)));

            if (phoneResponse.OperationStatus.RequestSuccessful)
            {
                return phoneResponse.Phones;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(phoneResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Phone details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddPhone(Phone args)
        {
            var phoneResponse = SvcClientOld.CallService<Phone, OperationStatus>(Constants.Phone, "POST", args);

            if (phoneResponse.RequestSuccessful)
            {
                return phoneResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(phoneResponse.Messages));
            }
        }

        /// <summary>
        /// Delete existing Phone details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeletePhone(PhoneRequestParms args)
        {
            var phoneResponse = SvcClientOld.CallService<PhoneRequestParms, OperationStatus>(Constants.Phone, "DELETE", args);

            if (phoneResponse.RequestSuccessful)
            {
                return phoneResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(phoneResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
