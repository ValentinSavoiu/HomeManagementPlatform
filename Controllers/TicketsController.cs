using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mss_project.DatabaseStuff;
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
			ticket.Creator = db.Members.Find(ticket.CreatorID);
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
		public ActionResult Create([Bind(Include = "TicketID,Title,Description,CreatorID,AssigneeID")] Ticket ticket)
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
		public ActionResult Edit([Bind(Include = "TicketID,Title,Status,Description,CreatorID,AssigneeID")] Ticket ticket)
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
