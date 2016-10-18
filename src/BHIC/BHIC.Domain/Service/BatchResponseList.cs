using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
	public class BatchResponseList
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

		public BatchResponseList()
		{
			// init lists to help avoid issues related to null reference exceptions
			BatchResponses = new List<BatchResponse>();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

		public List<BatchResponse> BatchResponses { get; set; }
	}
}
