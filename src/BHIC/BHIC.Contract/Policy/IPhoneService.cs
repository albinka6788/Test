#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IPhoneService
    {
        /// <summary>
        /// Returns list of Phone based on QuoteId,ContactId and PhoneId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Phone> GetPhoneList(PhoneRequestParms args);

        /// <summary>
        /// Save new Phone details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddPhone(Phone args);

        /// <summary>
        /// Delete existing Phone details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeletePhone(PhoneRequestParms args);
    }
}
