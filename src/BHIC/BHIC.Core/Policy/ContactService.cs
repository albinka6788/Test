#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Contract.Provider;
using BHIC.Domain.Policy;
using BHIC.Domain.Service;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class ContactService : IServiceProviders, IContactService
    {
        #region Methods

        #region Public Methods

        public ContactService (ServiceProvider provider)
	    {
            base.ServiceProvider = provider;
	    }

        /// <summary>
        /// Returns list of Contact based on QuoteId and ContactId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Contact> GetContactList(ContactRequestParms args)
        {
            var contactResponse = SvcClient.CallService<ContactResponse>(string.Concat(Constants.Contact,
                UtilityFunctions.CreateQueryString<ContactRequestParms>(args)), ServiceProvider);            

            if (contactResponse.OperationStatus.RequestSuccessful)
            {
                return contactResponse.Contacts;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(contactResponse.OperationStatus.Messages));
            }
        }

        /// <summary>
        /// Save new Contact details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus AddContact(Contact args)
        {            
            var contactResponse = SvcClient.CallServicePost<Contact, OperationStatus>(Constants.Contact, args, ServiceProvider);

            if (contactResponse.RequestSuccessful)
            {
                return contactResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(contactResponse.Messages));
            }
        }

        /// <summary>
        /// Save new Contact details
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public OperationStatus DeleteContact(ContactRequestParms args)
        {
            var contactResponse = SvcClientOld.CallService<ContactRequestParms, OperationStatus>(Constants.Contact, "DELETE", args);

            if (contactResponse.RequestSuccessful)
            {
                return contactResponse;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(contactResponse.Messages));
            }
        }

        #endregion

        #endregion
    }
}
