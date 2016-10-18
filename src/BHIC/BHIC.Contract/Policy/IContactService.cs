#region Using directives

using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IContactService
    {
        /// <summary>
        /// Returns list of Contact based on QuoteId and ContactId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Contact> GetContactList(ContactRequestParms args);

        /// <summary>
        /// Save new Contact details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus AddContact(Contact args);

        /// <summary>
        /// Delete existing Contact details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        OperationStatus DeleteContact(ContactRequestParms args);
    }
}
