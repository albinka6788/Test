using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
    public class BusinessTypeResponse
    {
        public List<BusinessType> BusinessTypes { get; set; }
        public OperationStatus OperationStatus { get; set; }

        public BusinessTypeResponse()
        {
            BusinessTypes = new List<BusinessType>();
            OperationStatus = new OperationStatus();
        }
    }
}
