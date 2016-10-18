using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    /// <summary>
    /// Parameters associated with the ClassDescKeyword service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ClassDescKeywordRequestParms
    {
        /// <summary>
        /// Return the specified ClassDescKeyword
        /// </summary>
        public int? ClassDescKeywordId { get; set; }

        /// <summary>
        /// Return the ClassDescKeyword(s) containing the SearchString text<br />
        /// Ignored if ClassDescKeywordId is specified.
        /// </summary>
        [StringLength(50)]
        public string SearchString { get; set; }

        /// <summary>
        /// Return the ClassDescKeyword(s) associated with the specified Line of Business (WC, BP, CA...)<br />
        /// Ignored if ClassDescKeywordId is specified.<br />
        /// Validation:<br />
        /// 1) Required if ClassDescKeywordId is not specified
        /// </summary>
        [StringLength(2)]
        public string LOB { get; set; }

        /// <summary>
        /// Return data for the specified State<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Return data for the State associated with the specified ZipCode<br />
        /// Validation: Either State or Zip code is required.<br />
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Background: By default, search terms are corrected in cases where simple typos occur (transposed characters, missing character, etc...) <br />
        ///  Set to true to disable keyword spell-checking.  This can be set to true under the following circumstances:<br />
        ///  a) If the client that consumes this service needs to disable spell-check functionality for some reason (i.e. - it accepts the fact that the value in SearchString may be incorrect, and doesn't want the service to check for typos.)<br />
        ///  b) The Insurance Service sets this property internally as needed. <br />
        /// </summary>
        public bool DisableSpellCheck { get; set; }

        /// <summary>
        /// Background: By default, search terms are tested to see if a synonym has been defined.  Example: user-entered search term Supplier can return keywords that contain the word 'WholeSaler'<br />
        ///  Set to true to disable keyword synonym-checking.  This can be set to true under the following circumstances:<br />
        ///  a) If the client that consumes this service needs to disable synonym-check functionality for some reason (i.e. - it accepts the fact that the value in SearchString may similar in nature to defined keywords, and doesn't want the service to check for defined synonyms.)<br />
        ///  b) The Insurance Service sets this property internally as needed. <br />
        /// </summary>
        public bool DisableSynonymCheck { get; set; }

        /// <summary>
        /// Background: By default, plural search terms are reduced to singular terms (which may result in more results, since many defined keywords are not plural).<br />
        ///  Set to true to disable keyword plural-checking.  This can be set to true under the following circumstances:<br />
        ///  a) If the client that consumes this service needs to disable plural-check functionality for some reason (i.e. - it accepts the fact that the value in SearchString may be plural, and doesn't want the service to check for keywords that are singluar.)<br />
        ///  b) The Insurance Service sets this property internally as needed. <br />
        /// </summary>
        public bool DisablePluralCheck { get; set; }

        /// <summary>
        ///  Set to true to disable all checks above.  This can be set to true under the following circumstances:<br />
        ///  a) If the client that consumes this service needs to disable checking functionality for some reason.<br />
        ///  b) The Insurance Service sets this property internally as needed. <br />
        /// </summary>
        public bool DisableAllChecks { get; set; }
    }
}
