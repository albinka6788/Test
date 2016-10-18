using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Domain.Policy
{
    public class WcLocNamCertificateResult
    {
        public string LocNum { get; set; }
        public string Type { get; set; }
        public string UseLocNum { get; set; }
        public string CertTypeDescrip { get; set; }
        public string LowDate { get; set; }
        public string HighDate { get; set; }
        public string Text { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string UseLocNumDescrip { get; set; }
        public DateTime EnteredOn { get; set; }
        public string EnteredBy { get; set; }
        //public int? DocumentId { get; set; }
        public BHIC.Domain.Document.Document Document { get; set; }

        public WcLocNamCertificateResult()
        {
            Document = new BHIC.Domain.Document.Document();
        }
    }
}