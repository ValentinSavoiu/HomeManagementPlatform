using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace mss_project.Models
{
	public class ChangeAssigneesViewModel
	{
		public Ticket currentTicket { get; set; }
		public List<Tuple<ApplicationUser, string>> Assignees { get; set; }
		public List<Tuple<ApplicationUser, string>> UnassignedMembers { get; set; }
	}

	public class TicketDetailsViewModel
	{
		public Ticket currentTicket { get; set; }
		public string CreatorNickName { get; set; }
		public List<string> AssigneesNickNames { get; set; }
		public List<Comment> Comments { get; set; }
		public List<string> CommentNicknames { get; set; }
		public ApplicationUser currentUser { get; set; }
		public Comment NewComment { get; set; }
	}

	public class TicketIndexViewModel
	{
		public Group currentGroup;
		public List<Ticket> tickets{ get; set; }
		public List<string> ticketCreatorNickNames { get; set; }
	}
}