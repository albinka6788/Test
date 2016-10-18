using System;
using System.Collections.Generic;
namespace BHIC.Domain.QuestionEngine
{
    public class BusinessInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public string TaxIdOrSSN { get; set; }
        public string TaxIdType { get; set; }
    }
    public class BusinessType 
    {
        public string AlternateDescription { get; set; }
        public string BusinessTypeCode { get; set; }
        public string Description { get; set; }
    }
}
