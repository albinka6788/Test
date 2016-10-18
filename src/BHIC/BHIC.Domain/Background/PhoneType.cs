using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	public class PhoneType
	{
		[StringLength(15)]
		public string PhoneTypeId { get; set; }

		[StringLength(128)]
		public string Description { get; set; }
	}
}
