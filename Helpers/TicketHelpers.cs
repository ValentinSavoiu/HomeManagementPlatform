using System.ComponentModel.DataAnnotations;
using System;
using System.Reflection;
using mss_project.Models;
using Newtonsoft.Json.Linq;
using mss_project.DatabaseStuff;
using System.Linq;
using System.Collections.Generic;

namespace mss_project.Helpers
{
	public static class TicketHelpers
	{
		public static string GetCreatorNickname(this Ticket ticket, JiraContext db)
		{
			string creatorNickName = db.GroupMembers.Where(x => x.Group_ID == ticket.GroupID && x.AppUser_ID == ticket.Creator.Id)
				.FirstOrDefault()?.NickName;

			if(creatorNickName == null)
			{
				creatorNickName = ticket.Creator.UserName + " (not member)";
			}

			return creatorNickName;
		}

		public static List<string> GetAssigneeNicknames(this Ticket ticket, JiraContext db)
		{
			List<string> assigneesNickNames = new List<string> { };

			foreach (var assignee in ticket.Assignees)
			{
				string assigneeNickName = db.GroupMembers.Where(x => x.Group_ID == ticket.GroupID && x.AppUser_ID == assignee.Id)
					.FirstOrDefault()?.NickName;

				if (assigneeNickName == null)
				{
					assigneeNickName = assignee.UserName + " (not member)";
				}

				assigneesNickNames.Add(assigneeNickName);
			}

			return assigneesNickNames;
		}
	}
}

