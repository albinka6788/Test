using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;
using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Background
{
    public class AvailableCarriersResponse
    {
        public AvailableCarriersResponse()
        {
            Carriers = new List<string>();
            OperationStatus = new OperationStatus();
        }

        [StringLength(100)]
        public List<string> Carriers { get; set; }

        public OperationStatus OperationStatus { get; set; }
    }
}
