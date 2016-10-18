using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Service;
using BHIC.Common.Client;
using BHIC.Domain.Dashboard;
using BHIC.Domain.Policy;

namespace BHIC.Contract.PolicyCentre
{
    public interface IRequestCertificateService
    {
        OperationStatus AddRequestCertificate(CertificateOfInsurance args);

        CertificateOfInsuranceResponse GetRequestCertificate(CertificateOfInsuranceRequestParms args);

    }
}
