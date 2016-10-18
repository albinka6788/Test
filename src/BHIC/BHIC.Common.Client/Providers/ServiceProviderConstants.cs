using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Common.Client
{
    /// <summary>
    /// Enum will provide all available API/Service providers name
    /// </summary>
    public enum ProviderNames
    {
        //Guard Insurance Company
        None = 0,

        //Guard Insurance Company
        Guard = 1,

        //National Council on Compensation Insurance (NCCI)
        NCCI = 2,

        //The Workers’ Compensation Insurance Rating Bureau of California (WCIRB)
        WCIRB = 3
    }

    /// <summary>
    /// Class will provide access to all generic and provider specific hard coded value or constants
    /// </summary>
    public class ServiceProviderConstants
    {
        #region Comment : Here "Guard" service provider constants

        //Comment : Here default "Config Section" name for Guard APIs
        public const string DefaultServiceConnectionNameGuard = "serviceConnections";

        //Comment : Here "Config Section ServiceElement" name for Guard APIs
        public const string DefaultGuardApiWC = "InsuranceApi";
        public const string GuardApiBOP = "InsuranceApiBOP";

        //Comment : Here provider specific service categories 
        public const string GuardServiceCategoryWC = "WC";
        public const string GuardServiceCategoryBOP = "BOP";
        
        #endregion

        #region Comment : Here "NCCI" service provider constants

        //Comment : Here default "Config Section" name for Guard APIs
        public const string DefaultServiceConnectionNameNCCI = "serviceConnectionsNCCI";

        //Comment : Here "Config Section ServiceElement" name for Guard APIs
        public const string DefaultNcciApi = "XModApi";
        public const string NcciApiOther = "OthersApi";

        //Comment : Here provider specific service categories 
        public const string NcciServiceCategoryXMod = "XMod";
        public const string NcciServiceCategoryOther = "Others";

        //Comment : Here provider api call optional parameter value
        public const string NcciTestParam = "TRUE";     //This will allow us to call without paying 12$ per call
        //Note : - Returns a canned mod response if value set to TRUE, or returns live records if you are carrier of record and the value set to COR.

        public const string NcciFormatParam = "J";      //Supported format types like Json,Xml etc
        public const string NcciModTypeParam = "C";      //Indicates what type of mod is being returned. Returns C for current, H for historical, and F for future.

        #endregion
    }
}
