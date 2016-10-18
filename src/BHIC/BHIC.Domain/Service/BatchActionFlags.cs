using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	/// <summary>
	/// For Insurance Service system internal use only.  Not used for typical requests.
	/// Used by the Insurance Service to set status within a given BatchAction, and retrieve that status from a subsequent BatchAction, 
	/// within the same Batch request (BatchActionList).   
	/// </summary>
	public class BatchActionFlags
	{
		public BatchActionFlags()
		{
			Values = new Dictionary<string, string>();
		}
		public Dictionary<string, string> Values { get; set; }
	}
}
