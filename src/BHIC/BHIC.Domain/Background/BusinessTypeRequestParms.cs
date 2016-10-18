using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	/// <summary>
	/// Parameters associated with the BusinessTypes service<br />
	/// Filters the response as indicated by the comments for each parameter
	/// </summary>
	public class BusinessTypeRequestParms
	{
		/// <summary>
		/// If true, BusinessTypes that are relevant to the policy's primary insured name (main name on the policy) will be returned.<br />
		/// If false, BusinessTypes that are relevant to additional names are returned.<br />
		/// See the InsuredNames POST parameter descriptions for additional information about primary insured name vs. additional names.<br />
		/// Validation:<br />
		/// 1) Required.<br />
		/// </summary>
		[Required]
		public bool InsuredNameTypesOnly { get; set; }
	}
}
