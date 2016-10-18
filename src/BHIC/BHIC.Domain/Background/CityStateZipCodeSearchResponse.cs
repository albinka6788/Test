using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class CityStateZipCodeSearchResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public CityStateZipCodeSearchResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            CityStateZipCodes = new List<CityStateZipCode>();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public List<CityStateZipCode> CityStateZipCodes { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
