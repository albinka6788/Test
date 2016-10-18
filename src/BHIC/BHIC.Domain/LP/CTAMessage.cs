using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.LP
{
    public class CTAMessage
    {
        public long Id { get; set; } //(bigint, not null)
        public string TokenId { get; set; } //(char(32), null)
        public string Message { get; set; } //(varchar(100), null)
        public DateTime? CreatedOn { get; set; }
    }
}
