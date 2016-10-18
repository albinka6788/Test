using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Background
{
	public class ContactType
	{
		//public int TYPE { get; set; }

		//[StringLength(1)]
		//public string CATEGORY { get; set; }

		//[StringLength(128)]
		//public string SQLTABLE { get; set; }

		//[StringLength(128)]
		//public string SQLFIELD { get; set; }

		//[StringLength(128)]
		//public string CONTACTGROUP { get; set; }

		//// establish relationship with ContactGroup
		//public virtual ContactGroups ContactGroups { get; set; }

		//public bool ALLOWMULTIPLES { get; set; }

		//public bool? ALLOWNEW { get; set; }

		//public bool? ALLOWDELETE { get; set; }

		//public bool? READONLY { get; set; }

		//[StringLength(10)]
		//public string CANEDIT { get; set; }

		//[StringLength(10)]
		//public string CANINSERT { get; set; }

		//[StringLength(10)]
		//public string CANDELETE { get; set; }

		[StringLength(128)]
		public string DESCRIPTION { get; set; }

		//public int SORTFIELD { get; set; }

		//public DateTime CREATED { get; set; }

		//[StringLength(8)]
		//public string CREATEDBY { get; set; }

		//public DateTime? MODIFIED { get; set; }

		//[StringLength(8)]
		//public string MODIFIEDBY { get; set; }

		[StringLength(15)]
		public string descid { get; set; }

		//[StringLength(10)]
		//public string canChange { get; set; }

	}
}
