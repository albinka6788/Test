using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.XMod
{
    public class RatingValuesInformation
    {
        public string RatingEffectiveDate { get; set; }
        public string RatingIssueDate { get; set; }
        public double ExperienceModificationFactor { get; set; }
        public string StatusOfRateFilingCode { get; set; }
        public string ContingentIndicator { get; set; }
        public string ModTypeCode { get; set; }
        public double AssignedRiskAdjustmentProgramFactor { get; set; }
        public double FloridaAssignedRiskAdjustmentProgramFactor { get; set; }
        public double MassachusettsAllRiskAdjustmentFactor { get; set; }
        public double SarapFactor { get; set; }
        public DateTime RatingEffectiveDateDateFormat { get; set; }
        public DateTime RatingIssueDateDateFormat { get; set; }
    }
}
