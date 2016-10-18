#region Using directives

using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.Policy;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Core.Policy
{
    public class DocumentService : IDocumentService
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Returns list of Document based on QuoteId,PolicyId and DocumentId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<Document> GetDocumentList(DocumentRequestParms args)
        {
            var documentResponse = SvcClientOld.CallService<DocumentResponse>(string.Concat(Constants.Document,
                UtilityFunctions.CreateQueryString<DocumentRequestParms>(args)));

            if (documentResponse.OperationStatus.RequestSuccessful)
            {
                return documentResponse.Documents;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(documentResponse.OperationStatus.Messages));
            }
        }

        #endregion

        #endregion
    }
}
