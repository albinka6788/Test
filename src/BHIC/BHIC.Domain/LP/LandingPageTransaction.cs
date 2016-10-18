using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.LP
{
    public class LandingPageTransaction
    {
        public long Id { get; set; } //(bigint, not null)
        [Display(Name = "LOB")]
        [Required]
        public string lob { get; set; } //(char(3), null)
        [Required]
        public string State { get; set; }  //(char(3), null)
        public long TransactionCounter { get; set; } //(bigint, null)
        [Display(Name = "Header Logo")]
        [Required]
        public string logo { get; set; } //(varchar(200), null)
        public string PageName { get; set; }
        [Display(Name = "Tag Line")]
        [Required]
        public string Heading { get; set; } //text
        public string SubHeading { get; set; } //text
        public string ProductName { get; set; } //text
        public string ProductHighlight { get; set; } //text
        public string BTNText { get; set; } //(varchar(200), null)
        public string CalloutText { get; set; } //text
        [Display(Name = "Landing Image")]
        [Required]
        public string MainImage { get; set; } //(varchar(100), null)
        public int TemplateId { get; set; } //(int, null)
        public string TemplateCss { get; set; } //(string, null)
        public string Controller { get; set; } //(varchar(50), null)
        public string ActionResult { get; set; } //(varchar(50), null)
        public string TokenId { get; set; } //(varchar(32), null)
        public string ZipCode { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedOn { get; set; }
        public bool ? IsDeployed { get; set; }
        public List<CTAMessage> CTAMsgList { get; set; }

    }
}
