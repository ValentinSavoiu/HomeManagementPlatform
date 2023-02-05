using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
	public class MembersAdminViewModel
	{
		public List<string> MembersID { get; set; }
		public List<string> GroupUsers { get; set; }
		public List<string> ListGroupNicknames { get; set; }

		public bool bIsOwner { get; set; }

		public int GroupID { get; set; }

	}
}