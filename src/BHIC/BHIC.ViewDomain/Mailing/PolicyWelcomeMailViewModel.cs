using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.ViewDomain.Mailing
{
    public class PolicyWelcomeMailViewModel : MailTemplatesBaseViewModel
    {
        //Basic communication/contact details
        public string PolicyCenterWebsiteUrlHref { get; set; }

        //Basic policy related details
        public string InsuredBusinessName { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime PolicyEffectiveDate { get; set; }
        public string PolicyEffectiveDateString { get; set; }
        public TimeSpan PolicyEffectiveTime { get; set; }

        //Policy billing related details
        public string TotalPremiumAmount { get; set; }
        public string PremiumAmountPaid { get; set; }
        public int PolicyInstalments { get; set; }
        public decimal PolicyNextInstallmentAmount { get; set; }
        public DateTime PolicyNextDueDate { get; set; }
        public string PolicyNextDueDateString { get; set; }
        public string NextInstallmentAmount { get; set; }
        public string StyleSingleInstallment { get; set; }
    }
}
