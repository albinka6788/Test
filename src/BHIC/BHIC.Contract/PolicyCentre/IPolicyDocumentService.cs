using BHIC.Common.Client;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IPolicyDocumentService
    {
        /// <summary>
        /// PolicyDocumentGet to get the list of Policy Documents
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<Document> PolicyDocumentGet(PolicyDocumentRequestParms args);
        
    }
}
