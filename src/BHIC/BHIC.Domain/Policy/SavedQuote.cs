using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
	/// <summary>
	/// Parameters associated with sending a 'Saved Quote' email to an end user.
	/// </summary>
	public class SavedQuote
	{
		/// <summary>
		/// Identifies the Quote that the user will have access to via Email.<br />
		/// Example: 123<br />
		/// </summary>
		[Required]
		public int? QuoteId { get; set; }

		/// <summary>
		/// Email Address to receive the saved quote message.<br />
		/// Example: john.smith@company.com<br />
		/// Validation:<br />
		/// 1) Required.<br />
		/// 2) Must be a valid email address<br />
		/// </summary>
		[Required]
		[StringLength(256)]
		public string EmailAddress { get; set; }

		/// <summary>
		/// URL that represents the target site that the user will return to when clicking the link in their email.<br />
		/// Example of a URL that could be provided in this parameter: "https://directsales.com/ReturnToQuote"<br />
		/// This URL will be formatted, along with a GUID that is created by the SavedQuote POST, into the email sent to the end user.<br />
		/// Convention used to format the link in the user's email: (ReturnToUrl parameter here)?returnToQuoteGUID=(GUID created by service here)<br />
		/// Example of a URL formatted into the user's email. "https://directsales.com/ReturnToQuote?returnToQuoteGUID=1c8b07db-54c1-41a3-9baf-50339491c3f3"<br />
		/// The ReturnToURL implemented in the UI will need to be able to process requests formatted as shown above.  For example the ReturnToUrl could process a request as follows:<br />
		/// a) Parse the returnToQuoteGUID parameter from the URI.<br />
		/// b) Pass the returnToQuoteGUID to the SavedQuote GET method to retrieve the associated QuoteId.
		/// c) Use the QuoteId returned above as a basis for calling other services required by the UI. 
		/// </summary>
		[Required]
		[StringLength(256)]
		public string ReturnToUrl { get; set; }	

		/// <summary>
		/// Link text displayed to user; used when formatting the link to be embedded in the user's email (see ReturnToUrl)
		/// </summary>
		[Required]
		[StringLength(256)]
		public string LinkText { get; set; }	

		/// <summary>
		/// Subject line of the email to be sent to the end user
		/// </summary>
		[Required]
		[StringLength(256)]
		public string Subject { get; set; }

		/// <summary>
		/// Prefix / first part of the message's body to be sent to the end user.<br />
		/// The message body sent to the end user will be formatted using the following convention:<br />
		/// (BodyPrefix Here)<br />
		/// (URL presented to the user for purposes of accessing their quote here - formatted by the service - see ReturnUrl for more information)<br />
		/// (BodySuffix Here)<br />
		/// </summary>
		[Required]
		[StringLength(10000)]
		public string BodyPrefix { get; set; }

		/// <summary>
		/// Suffix / trailing part of the message's body to be sent to the end user.<br />
		/// The message body sent to the end user will be formatted using the following convention:<br />
		/// (BodyPrefix Here)<br />
		/// (URL presented to the user for purposes of accessing their quote here - formatted by the service - see ReturnUrl for more information)<br />
		/// (BodySuffix Here)<br />
		/// </summary>
		[Required]
		[StringLength(10000)]
		public string BodySuffix { get; set; }
	}
}
