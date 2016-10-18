using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.ViewDomain
{
    public class ReferralQuoteViewModel
    {
        #region Comment : Here constructor / initialization

        public ReferralQuoteViewModel()
        {
            // init objects to help avoid issues related to null reference exceptions
            ContactName = string.Empty;
            BusinessName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration
        
        public string ContactName { get; set; }
        public string BusinessName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        #endregion
    }

    public class ReferralHistory
    {
        #region Comment : Here constructor / initialization

        public ReferralHistory()
        {
            // init objects to help avoid issues related to null reference exceptions
            ReferralScenarioIdsList = new List<List<int>>();
            ReferralScenarioTextList = new List<ReferralData>();
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public List<List<int>> ReferralScenarioIdsList { get; set; }
        public List<ReferralData> ReferralScenarioTextList { get; set; }
        public string XModValueMessage { get; set; }

        #endregion
    }

    public class ReferralData
    {
        #region Comment : Here constructor / initialization

        public ReferralData()
        {
            // init objects to help avoid issues related to null reference exceptions
            ReasonsList = new List<string>();
            DescriptionList = new List<string>();
        }

        #endregion

        #region Variables : Page Level Local Variables Decalration

        public List<string> ReasonsList { get; set; }
        public List<string> DescriptionList { get; set; }

        #endregion
    }
}
