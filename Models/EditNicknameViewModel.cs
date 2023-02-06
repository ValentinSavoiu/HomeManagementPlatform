using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
	public class EditNicknameViewModel
	{
		public GroupMember CurrentMember { get; set; }
		public string MemberUsername { get; set; }
	}
}