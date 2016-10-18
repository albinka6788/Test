using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
    public class GuardCDNBundle
    {
        public CDNBundleType BundleType { get; set; }
        public string Version { get; set; }
        public string Contents { get; set; }
        public string FullDebugPath { get; set; }
    }
}
