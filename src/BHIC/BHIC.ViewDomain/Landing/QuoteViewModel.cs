#region Using directives

using BHIC.Domain.Background;
using BHIC.Domain.Policy;
using System;
using System.Collections.Generic;


#endregion

namespace BHIC.ViewDomain.Landing
{
    public class YearsInBusiness
    {
        public string value { get; set; }
        public string text { get; set; }
    }
    public class QuoteViewModel
    {

        public int SelectedSearch { get; set; }
        public County County { get; set; }

        public decimal AnnualPayroll { get; set; }

        public int? ClassDescriptionId { get; set; }

        public int? ClassDescriptionKeywordId { get; set; }

        public string ClassCode { get; set; }

        public bool IsMultiState { get; set; }

        public Industry Industry { get; set; }

        public SubIndustry SubIndustry { get; set; }

        public ClassDescription Class { get; set; }

        public string InceptionDate { get; set; }

        public int? IndustryId { get; set; }

        public int? SubIndustryId { get; set; }

        public bool IsMultiClass { get; set; }

        public PolicyData PolicyData { get; set; }

        public YearsInBusiness SelectedYearInBusiness { get; set; }

        public int BusinessYears { get; set; }

        public List<Exposure> Exposures { get; set; }

        public List<int> LobDataIds { get; set; }

        public List<int> CoverageStateIds { get; set; }

        public bool? IsGoodState { get; set; }

        public string BusinessName { get; set; }

        public bool IsMultiClassPrimaryExposureValid { get; set; }

        public bool IsGoodStateApplicable { get; set; }

        public bool IsMultiClassApplicable { get; set; }

        public bool IsMultiStateApplicable { get; set; }

        public string TotalPayroll { get; set; }

        public bool MoreClassRequired { get; set; }

        public List<CompanionClassData> CompClassData { get; set; }

        public decimal MinExpValidationAmount { get; set; }

        //For BusinessClass DirectSales value
        public string BusinessClassDirectSales { get; set; }

        public string OtherClassDesc { get; set; }


        public List<Industry> Industries { get; set; }

        public List<SubIndustry> SubIndustries { get; set; }

        public List<ClassDescription> Classes { get; set; }

        public List<YearsInBusiness> YearsInBusinessList
        {
            get;
            set;
        }


        public string IndustryName { get; set; }

        public string SubIndustryName { get; set; }

        public string BusinessYearsText { get; set; }

        public List<NavigationModel> NavigationLinks { get; set; }

        public string QuoteId { get; set; }

        public bool MinPayrollAllResponseRecieved { get; set; }

        public bool IsValidQuestionPageRequest { get; set; }

        public string ProspectInfoEmail { get; set; }
    }
}
