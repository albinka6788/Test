using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	public class BatchActionList
	{

		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public BatchActionList()
		{
			// init lists to help avoid issues related to null reference exceptions
			BatchActions = new List<BatchAction>();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		/// <summary>
		/// List of BatchActions to submit to the Insurance Service
		/// </summary>
		public List<BatchAction> BatchActions { get; set; }
	}
}
