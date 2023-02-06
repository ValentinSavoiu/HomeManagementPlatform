using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
	public class GroupsAdministratorViewModel
	{
		public ApplicationUser CurrentUser { get; set; }
		public List<Group> ListGroupsJoined { get; set; }

	}
}