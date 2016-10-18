using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Contract.Policy;
using BHIC.Common;
using BHIC.Domain.Policy;
using BHIC.Common.Config;
using BHIC.Domain.Service;
using BHIC.Contract.Provider;
using BHIC.Common.Client;
using BHIC.Contract.PolicyCentre;
using BHIC.Domain.Dashboard;

namespace BHIC.Core.PolicyCentre
{
    public class RequestCertificateService : IServiceProviders, IRequestCertificateService
    {

        CertRequestResponse certRequestResponse = new CertRequestResponse();

        public RequestCertificateService(ServiceProvider provider)
        {
            base.ServiceProvider = provider;
        }

        public OperationStatus AddRequestCertificate(CertificateOfInsurance args)
        {
            var operationStatus = SvcClient.CallServicePost<CertificateOfInsurance, OperationStatus>(Constants.RequestCertificate, args, ServiceProvider);
            return operationStatus;
        }

        public CertificateOfInsuranceResponse GetRequestCertificate(CertificateOfInsuranceRequestParms args)
        {
           CertificateOfInsuranceResponse certificateOfInsuranceResponse = SvcClient.CallService<CertificateOfInsuranceResponse>(string.Concat(Constants.RequestCertificate,
                UtilityFunctions.CreateQueryString<CertificateOfInsuranceRequestParms>(args)), ServiceProvider);
            return certificateOfInsuranceResponse;
        }


       
    }
}