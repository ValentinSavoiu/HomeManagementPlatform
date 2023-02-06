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
		public List<UserViewModel> UserInfo { get; set; }
		public List<string> ListGroupNicknames { get; set; }
		public Group Group { get; set; }
		public UserViewModel CurrUser { get; set; }
	}
}