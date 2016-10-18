using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public class Insured
    {
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string CityStateZip { get; set; }

        public Insured() { }

        public Insured(string name, string address, string citystatezip)
        {
            Name = name;
            Address = address;
            CityStateZip = citystatezip;
        }
    }
}
