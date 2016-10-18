using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Document
{
    public class Document
    {
        public int DocumentId { get; set; }
        public string FullFilePath { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public string Rule { get; set; }
        public string RuleFriendlyName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Display { get; set; }
        public string EncryptedDocumentId { get; set; }
    }
}
