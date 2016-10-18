#region Using directives

using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Policy
{
    public interface IDocumentService
    {
        /// <summary>
        /// Returns list of Document based on QuoteId,PolicyId and DocumentId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Document> GetDocumentList(DocumentRequestParms args);
    }
}
