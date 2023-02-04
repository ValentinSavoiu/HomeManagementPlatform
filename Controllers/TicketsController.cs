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
using mss_project.DatabaseStuff;
using mss_project.Helpers;
using mss_project.Models;

namespace mss_project.Controllers
{
	public class TicketsController : Controller
	{
		private JiraContext db = new JiraContext();

		// GET: Tickets
		public ActionResult Index()
		{
			List<Ticket> tickets = db.Tickets.ToList();

			foreach (var ticket in tickets)
			{
				ticket.Creator = db.Members.Find(ticket.CreatorID);
			}
			return View(tickets);
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
			return View(ticket);
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
		public ActionResult Edit([Bind(Include = "TicketID,Title,Status,Description,CreatorID")] Ticket ticket)
		{
			if (ModelState.IsValid)
			{
				db.Entry(ticket).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
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
			return View(ticket);
		}

		// POST: Tickets/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Ticket ticket = db.Tickets.Find(id);
			db.Tickets.Remove(ticket);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		// GET: Tickets/ChangeAssignees/5
		public ActionResult ChangeAssignees(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Ticket ticket = db.Tickets.Find(id);
			var model = new ChangeAssigneesViewModel();
			model.TicketID = ticket.TicketID;
			model.Assignees = ticket.Assignees.ToList();
			model.UnassignedMembers = db.Members.ToList().Except(model.Assignees).ToList();

			if (ticket == null)
			{
				return HttpNotFound();
			}
			return View(model);
		}

		// POST: Tickets/AddAssignee?ticketID=2
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddAssignee(int ticketID, int? memberID)
		{
			if(memberID == null)
			{
				return RedirectToAction("ChangeAssignees", new { id = ticketID });
			}

			Member newAssignee = db.Members.Find(memberID);
			Ticket ticket = db.Tickets.Find(ticketID);
			ticket.Assignees.Add(newAssignee);
			db.SaveChanges();

			// Send an email to this member in order to receive a notification about this ticket assignment
			var callbackUrl = Url.Action("Details", "Tickets", new { id = ticketID, }, protocol: Request.Url.Scheme);
			var receiverEmail = new MailAddress(newAssignee.Email, "Receiver");
			var subject = "Ticket assignment";
			var body = "You have been assigned to ticket " + "\"" + ticket.Title + "\". " + "To see more details, visit the following link:\n" + callbackUrl;
			EmailSender emailSender = EmailSender.getInstance("C:\\Users\\Me\\Desktop\\secret_store");
			emailSender.sendEmail(receiverEmail.Address, subject, body);

			return RedirectToAction("ChangeAssignees", new { id = ticketID });
		}

		// POST: Ticket/RemoveAssignee?ticketId=1&memberId=1
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveAssignee(int ticketID, int memberID)
		{
			Ticket ticket = db.Tickets.Find(ticketID);
			Member assignee = db.Members.Find(memberID);
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
