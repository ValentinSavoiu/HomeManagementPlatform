using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
	public class ChangeAssigneesViewModel
	{
		public int TicketID { get; set; }
		public List<Member> Assignees { get; set; }
		public List<Member> UnassignedMembers { get; set; }
	}
}