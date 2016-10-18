using BHIC.Domain.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.Dashboard
{
    public interface ICertificateOfInsuranceService
    {
        bool CreateCertificateofInsurance(CertificateOfInsuranceDTO certificate);
        List<string> GetCertificateofInsurance(string policyNumber);

    }
}
