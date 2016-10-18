using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Dashboard
{
    public class UploadDocumentDTO
    {
        public string description { get; set; }
        public string BusinessName { get; set; }
        public string policyCode { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
    }
}
