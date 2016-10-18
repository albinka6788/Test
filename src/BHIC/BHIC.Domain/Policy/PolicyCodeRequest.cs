using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class PolicyCodeRequest
    {
        [StringLength(255)]
        public string PolicyCode { get; set; }
    }
}
