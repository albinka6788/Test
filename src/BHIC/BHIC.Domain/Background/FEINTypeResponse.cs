using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
    public class FEINTypeResponse
    {
        public List<FEINType> FEINTypes { get; set; }
        public OperationStatus OperationStatus { get; set; }

        public FEINTypeResponse()
        {
            FEINTypes = new List<FEINType>();
            OperationStatus = new OperationStatus();
        }
    }
}
