using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
	public class VCityStateZipCodeResponse
	{
		// ----------------------------------------
		// constructor
		// ----------------------------------------

        public VCityStateZipCodeResponse()
		{
			// init objects to help avoid issues related to null reference exceptions
            VCityStateZipCodeRequestParms = new VCityStateZipCodeRequestParms();
			OperationStatus = new OperationStatus();
		}

		// ----------------------------------------
		// properties
		// ----------------------------------------

        public VCityStateZipCodeRequestParms VCityStateZipCodeRequestParms { get; set; }
		public OperationStatus OperationStatus { get; set; }
	}
}
