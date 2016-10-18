using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;
using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Background
{
    public class ClassDescKeywordResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public ClassDescKeywordResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            ClassDescKeywords = new List<ClassDescriptionKeyword>();
            SuggestionList = new List<string>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// List of ClassDescKeywords associated with the passed SearchString<br />
        /// In the event that spell-checking detects a potential typo, this may be populated with keywords associated with alternate words that spell-checking recommends.  See SearchString and SuggestionList for more information.
        /// </summary>
        public List<ClassDescriptionKeyword> ClassDescKeywords { get; set; }

        /// <summary>
        /// Search String submitted to the ClassDescKeywords GET request<br />
        /// In the event that spell-checking detects a potential typo, it may be useful to have access to the original SearchString that contained the typo.
        /// </summary>
        [StringLength(50)]
        public string SearchString { get; set; }

        /// <summary>
        /// In the even tthat spell-checking detects a potential typo, this property will contain a list of words that are similar to the search string which would result in the return of ClassDescKeywords.<br />
        /// This may be useful in displaying messages to the user, such as the following:  "Unable to locate a business type related to 'rofer'; displaying results that contain 'roper' and 'roofer', instead"
        /// </summary>
        public List<string> SuggestionList { get; set; }

        public OperationStatus OperationStatus { get; set; }
    }
}
