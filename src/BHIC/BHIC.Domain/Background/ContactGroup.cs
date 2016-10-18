using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	public class ContactGroups
	{
		[StringLength(50)]
		public string ContactGroup { get; set; }

		// establish relationship with ContactType
		public virtual ICollection<ContactType> ContactTypes { get; set; }

		[StringLength(1000)]
		public string Descrip { get; set; }

		[StringLength(1000)]
		public string Display { get; set; }

		public int SortOrder { get; set; }
	}
}
