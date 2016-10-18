using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public enum DocumentGrouping
    {
        // Core Policy Documents
        PolicyDocuments = 1,

        // Billing Documents
        BillingStatements = 11,
        DeductibleInvoices = 12,
        
        // Additional Policy Documents
        CertificatesOfInsurance = 21,
        PhysicianPanels = 22
    }
}