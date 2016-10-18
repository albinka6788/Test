using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class ViewDocument
    {
        public string Filename { get; set; }
        public string FileExt { get; set; }
        public byte[] Contents { get; set; }
    }
}