using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity; 
using mss_project.DatabaseStuff;
using System.Net.Mail;
using mss_project.Models;
using System.Diagnostics;
using mss_project.Helpers;

namespace mss_project.Controllers
{
    public class GroupsController : Controller
    {
        private JiraContext db = new JiraContext();
        private ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        // GET: Groups
        public ActionResult Index()
        {
            var Owner = userManager.Users.ToList().Find(x => x.Id == User.Identity.GetUserId());
            var AllJoinedGroups = db.GroupMembers.ToList().FindAll(x => x.AppUser_ID == Owner.Id).ConvertAll(x=>x.Group);
            Debug.WriteLine(AllJoinedGroups);
            Debug.WriteLine(Owner.Email.ToString());
            return View(new GroupsAdministratorViewModel{ CurrentUser = Owner, ListGroupsJoined = AllJoinedGroups });
        }

        // GET: Groups/Members/5
        public ActionResult Members(int? id)
        {
            var CurrUserApp = userManager.Users.ToList().Find(x => x.Id == User.Identity.GetUserId());
            var CurrUser = new UserViewModel {
                Id = CurrUserApp.Id,
                Email = CurrUserApp.Email,
                UserName = CurrUserApp.UserName
            };
			var AllGroupMembers = db.GroupMembers.ToList().FindAll(x => x.Group_ID == id);
            List<ApplicationUser> Members = new List<ApplicationUser> { };
            for(int i = 0; i < AllGroupMembers.Count; i++)
            {
				ApplicationUser user = userManager.Users.ToList().Find(x => x.Id == AllGroupMembers[i].AppUser_ID);
				Members.Add(user);
            }

			var UserInfo = Members.Select(x => new UserViewModel {
				Id = x.Id,
                Email = x.Email,
                UserName = x.UserName
			}).ToList();

			var AllNicknames = AllGroupMembers.ConvertAll(x => x.NickName);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            return View(new MembersAdminViewModel { UserInfo = UserInfo, ListGroupNicknames = AllNicknames, CurrUser = CurrUser, Group = group});
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                var Owner = userManager.Users.ToList().Find(x => x.Id == User.Identity.GetUserId());
                group.OwnerEmail = Owner.Email;
                db.Groups.Add(group);
                db.SaveChanges();
                db.GroupMembers.Add(new GroupMember { AppUser_ID = Owner.Id, Group_ID = group.GroupID, NickName = Owner.UserName });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult ChangeName(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeName([Bind(Include = "GroupID,Name,OwnerEmail")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Groups/Exit/5
        [HttpPost, ActionName("Exit")]
        [ValidateAntiForgeryToken]
        public ActionResult ExitConfirmed(int id_group)
        {
            var Owner = userManager.Users.ToList().Find(x => x.Id == User.Identity.GetUserId());
            GroupMember memb = db.GroupMembers.ToList().Find(x => x.AppUser_ID == Owner.Id && x.Group_ID == id_group);
            db.GroupMembers.Remove(memb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Groups/Kick/5
        [HttpPost, ActionName("Kick")]
        [ValidateAntiForgeryToken]
        public ActionResult KickConfirmed(int id_group, string memberID)
        {
            var Owner = userManager.Users.ToList().Find(x => x.Id == User.Identity.GetUserId());
            GroupMember memb = db.GroupMembers.ToList().Find(x => x.AppUser_ID == memberID && x.Group_ID == id_group);
            db.GroupMembers.Remove(memb);
            db.SaveChanges();
            return RedirectToAction("Members", new { id = id_group });
        }

        // POST: Groups/AddMember/5
        [HttpPost, ActionName("AddMember")]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(int GroupID, string email)
        {
            var NewMemb = "";
            var username = "";
            if (userManager.Users.ToList().FindAll(x => x.Email == email).Count != 0)
            {
                NewMemb = userManager.Users.ToList().Find(x => x.Email == email).Id;
				username = userManager.Users.ToList().Find(x => x.Email == email).UserName;
			}
            else
            {
                return RedirectToAction("Members", new { id = GroupID });
            }
            Group group = db.Groups.Find(GroupID);
            if (db.GroupMembers.ToList().FindAll(x=> x.Group_ID == GroupID && x.AppUser_ID == NewMemb).Count != 0)
            {
                return RedirectToAction("Members", new { id = GroupID });
            }
            // send email
            var callbackUrl = Url.Action("Members", "Groups", new { id = GroupID, }, protocol: Request.Url.Scheme);
            var receiverEmail = new MailAddress(email, "Receiver");
            var subject = "Request join group";
            var body = "You have been requested to join the group " + "\"" + group.Name + "\". " + "To see more details, visit the following link:\n" + callbackUrl;
            EmailSender emailSender = EmailSender.getInstance();
            emailSender.sendEmail(receiverEmail.Address, subject, body);

            GroupMember memb = new GroupMember { AppUser_ID = NewMemb, Group_ID = GroupID, NickName = username };
           
            db.GroupMembers.Add(memb);
            db.SaveChanges();
            return RedirectToAction("Members", new { id = GroupID });
        }

        // GET: Groups/Delete/5
        public ActionResult EditNickname(string id, int GroupID)
        {
            if (id == null || GroupID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMember groupMem = db.GroupMembers.ToList().Find(x => x.AppUser_ID == id && x.Group_ID == GroupID);
            var MemberUsername = userManager.Users.ToList().Find(x => x.Id == groupMem.AppUser_ID).UserName;

			if (groupMem == null)
            {
                return HttpNotFound();
            }
            return View(new EditNicknameViewModel{ CurrentMember = groupMem , MemberUsername = MemberUsername });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNickname([Bind(Include = "Group_ID,AppUser_ID,NickName")] GroupMember group)
        {
			var MemberUsername = userManager.Users.ToList().Find(x => x.Id == group.AppUser_ID).UserName;
			if (ModelState.IsValid)
            {
                if (group.NickName == null)
                {
					return View(new EditNicknameViewModel { CurrentMember = group, MemberUsername = MemberUsername });
				}
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Members", new { id = group.Group_ID });
            }
			return View(new EditNicknameViewModel { CurrentMember = group, MemberUsername = MemberUsername });
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
