using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Policy
{
    public class CertificateOfInsuranceResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public CertificateOfInsuranceResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            CertificatesOfInsurance = new List<CertificateOfInsurance>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<CertificateOfInsurance> CertificatesOfInsurance { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}
