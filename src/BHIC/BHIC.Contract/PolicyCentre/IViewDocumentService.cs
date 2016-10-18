using BHIC.Common.Client;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IViewDocumentService
    {
        /// <summary>
        /// ViewDocument to get the Documents
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ViewDocument ViewDocument(ViewDocumentRequestParms args);
    }
}
