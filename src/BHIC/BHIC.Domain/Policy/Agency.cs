using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Agency
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Agency() { }

        public Agency(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
