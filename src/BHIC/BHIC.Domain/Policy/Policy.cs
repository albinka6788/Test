using BHIC.Domain.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Policy
    {
        public PolicyDetails PolicyDetails { get; set; }
        public Billing Billing { get; set; }
        public List<SystemVariable> SystemVariables { get; set; }
        public List<Document> PolicyDocuments { get; set; }
        public List<Document> PhysicianPanels { get; set; }
        public List<CertificateOfInsurance> CertificatesOfInsurance { get; set; }

        public Policy()
        {
            PolicyDetails = new PolicyDetails();
            Billing = new Billing();
            SystemVariables = new List<SystemVariable>();
            PolicyDocuments = new List<Document>();
            PhysicianPanels = new List<Document>();
            CertificatesOfInsurance = new List<CertificateOfInsurance>();
        }
    }
}
