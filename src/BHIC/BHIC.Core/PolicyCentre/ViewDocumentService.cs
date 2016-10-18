using BHIC.Common;
using BHIC.Common.Client;
using BHIC.Common.Config;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.Provider;
namespace BHIC.Core.PolicyCentre
{
    public class ViewDocumentService : IServiceProviders, IViewDocumentService
    {
        public ViewDocumentService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }
        /// <summary>
        /// View Documents based on DocumentId, PolicyCode and SessionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ViewDocument ViewDocument(ViewDocumentRequestParms args)
        {
            var viewDocumentResponse = SvcClient.CallService<ViewDocumentResponse>(string.Concat(Constants.ViewDocument,
                UtilityFunctions.CreateQueryString<ViewDocumentRequestParms>(args)), ServiceProvider);

            if (viewDocumentResponse.OperationStatus.RequestSuccessful)
            {
                return viewDocumentResponse.Document;
            }
            else
            {
                throw new ApplicationException(UtilityFunctions.ConvertMessagesToString(viewDocumentResponse.OperationStatus.Messages));
            }
        }
    }
}
