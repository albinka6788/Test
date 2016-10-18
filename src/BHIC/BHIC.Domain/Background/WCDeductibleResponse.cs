#region Using directives

using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.Background
{
    public class WCDeductibleResponse
    {
        public WCDeductibleResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            Deductibles = new List<WCDeductibles>();
            DeductibleTypes = new List<WCDeductibleType>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<WCDeductibles> Deductibles { get; set; }
        public OperationStatus OperationStatus { get; set; }
        public List<WCDeductibleType> DeductibleTypes { get; set; }
    }
}
