using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Document
    {
        public int? DocumentId { get; set; }

        [StringLength(3)]
        public string FileExt { get; set; }

        [StringLength(1000)]
        public string FileName { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public string EncryptedDocumentId { get; set; }
    }
}
