using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using mss_project.DatabaseStuff;
using mss_project.Helpers;
using mss_project.Models;

namespace mss_project.Controllers
{
	public class TicketsController : Controller
	{
		private JiraContext db = new JiraContext();
		private ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

		// GET: Tickets
		public ActionResult Index()
		{
			List<Ticket> tickets = db.Tickets.ToList();
			List<string> ticketCreatorNickNames = new List<string> { };
			foreach (var ticket in tickets)
			{
				ticketCreatorNickNames.Add(ticket.GetCreatorNickname(db));
			}

			return View(new TicketIndexViewModel { tickets = tickets, ticketCreatorNickNames = ticketCreatorNickNames});
		}

		// GET: Tickets/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Ticket ticket = db.Tickets.Find(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}

			return View(new TicketDetailsViewModel { currentTicket = ticket, CreatorNickName = ticket.GetCreatorNickname(db), AssigneesNickNames = ticket.GetAssigneeNicknames(db) });
		}

		// GET: Tickets/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Tickets/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "TicketID,Title,Description,CreatorID")] Ticket ticket)
		{
			if (ModelState.IsValid)
			{
				db.Tickets.Add(ticket);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(ticket);
		}

		// GET: Tickets/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ticket ticket = db.Tickets.Find(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(ticket);
		}

		// POST: Tickets/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "TicketID,GroupID,CreatorID,Title,Status,Description")] Ticket ticket)
		{
			if (ModelState.IsValid)
			{
				db.Entry(ticket).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Tickets", "Groups", new {id = ticket.GroupID});
			}
			return View(ticket);
		}

		// GET: Tickets/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Ticket ticket = db.Tickets.Find(id);
			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(new TicketDetailsViewModel { currentTicket = ticket, CreatorNickName = ticket.GetCreatorNickname(db), AssigneesNickNames = ticket.GetAssigneeNicknames(db) }); ;
		}

		// POST: Tickets/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Ticket ticket = db.Tickets.Find(id);
			db.Tickets.Remove(ticket);
			db.SaveChanges();
			return RedirectToAction("Tickets", "Groups", new { id = ticket.GroupID });
		}

		// GET: Tickets/ChangeAssignees/5
		public ActionResult ChangeAssignees(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Ticket ticket = db.Tickets.Find(id);

			var unassignedMembers = db.GroupMembers.Where(x => x.Group_ID == ticket.GroupID).Select(x => x.User).ToList()
				.Except(ticket.Assignees).ToList();

			Ticket dummyTicket = new Ticket { GroupID = ticket.GroupID, Assignees = unassignedMembers };

			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(new ChangeAssigneesViewModel {
				currentTicket = ticket, 
				Assignees = ticket.Assignees.ToList().Zip(ticket.GetAssigneeNicknames(db), (first, second) => new Tuple<ApplicationUser, string> (first, second)).ToList(), 
				UnassignedMembers = unassignedMembers.Zip(dummyTicket.GetAssigneeNicknames(db), (first, second) => new Tuple<ApplicationUser, string>(first, second)).ToList(), 
			});
		}

		// POST: Tickets/AddAssignee?ticketID=2
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddAssignee(int ticketID, string userID)
		{
			if(userID == "")
			{
				return RedirectToAction("ChangeAssignees", new { id = ticketID });
			}

			ApplicationUser newAssignee = db.ApplicationUsers.Where(x => x.Id == userID).Single();
			Ticket ticket = db.Tickets.Find(ticketID);
			ticket.Assignees.Add(newAssignee);
			db.SaveChanges();

			// Send an email to this member in order to receive a notification about this ticket assignment
			var callbackUrl = Url.Action("Details", "Tickets", new { id = ticketID, }, protocol: Request.Url.Scheme);
			var receiverEmail = new MailAddress(newAssignee.Email, "Receiver");
			var subject = "Ticket assignment";
			var body = "You have been assigned to ticket " + "\"" + ticket.Title + "\" from group "+ "\"" + ticket.Group.Name + "\"" + ". " + "To see more details, visit the following link:\n" + callbackUrl;
			EmailSender emailSender = EmailSender.getInstance();
			emailSender.sendEmail(receiverEmail.Address, subject, body);

			return RedirectToAction("ChangeAssignees", new { id = ticketID });
		}

		// POST: Ticket/RemoveAssignee?ticketId=1&userId="aaa-bbb-ccc"
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveAssignee(int ticketID, string userID)
		{
			Ticket ticket = db.Tickets.Find(ticketID);
			ApplicationUser assignee = db.ApplicationUsers.Where(x => x.Id == userID).Single();
			ticket.Assignees.Remove(assignee);
			db.SaveChanges();
			return RedirectToAction("ChangeAssignees", new { id = ticketID });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
