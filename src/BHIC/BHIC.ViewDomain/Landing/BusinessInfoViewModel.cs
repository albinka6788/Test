#region Using directives
		
using BHIC.Domain.QuestionEngine;
using BHIC.ViewDomain.Landing;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.ViewDomain.Landing
{
    public class BusinessInfoViewModel
    {
        #region Comment : Here constructor / initialization

        public BusinessInfoViewModel()
        {
            // initialize Questions lookups
            CompanyName = string.Empty;
            ZipCode = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            ContactName = string.Empty;
            Address1 = string.Empty;
            City = string.Empty;
            StateCode = string.Empty;
            LobId = string.Empty;
            List<string> Messages = new List<string>();
            ProspectInfoId = null;
            LobList = string.Empty;
            CityList = string.Empty;
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public string CompanyName { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactName { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string LobId { get; set; }
        public List<string> Messages { get; set; }
        //Keep prospactInfo id if submitted once
        public int? ProspectInfoId { get; set; }
        public string LobList { get; set; }
        public string CityList { get; set; }
        public bool IsMultiStateOrMultiClass { get; set; }
        #endregion
    }

    //public class LineOfBusiness
    //{
    //    public Int32 Id { get; set; }
    //    public string Abbreviation { get; set; }
    //    public string LobFullName { get; set; }
    //    public string StateCode { get; set; }
    //    public Int32 StateLineOfBusinessId { get; set; }
    //    public string Status { get; set; }
    //}
}
