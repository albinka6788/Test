using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	public class LocationType
	{
		[StringLength(1)]
		public string LocationTypeId { get; set; }

		[StringLength(128)]
		public string Description { get; set; }
	}
}
