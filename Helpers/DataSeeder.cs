using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using mss_project;
using mss_project.DatabaseStuff;
using mss_project.Models;

namespace mss_project.Helpers
{
	public class DataSeeder
	{
		private ApplicationUserManager userManager;
		private JiraContext dbContext;
		private static DataSeeder instance = null;
		private DataSeeder()
		{
			var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
			userManager = new ApplicationUserManager(userStore);

			dbContext = new JiraContext();
		}

		public static DataSeeder getInstance()
		{
			if (instance == null)
			{
				instance = new DataSeeder();
			}
			return instance;
		}

		public async Task SeedData()
		{
			bool isCreated = dbContext.Database.CreateIfNotExists();

			if (isCreated)
			{
				Debug.WriteLine("Database created!");

				// Discriminator is a column that is automatically created whenever a model is a derived class
				// it is not required because only ApplicationUser inherits from IdentityUser
				dbContext.Database.ExecuteSqlCommand("ALTER TABLE AspNetUsers DROP COLUMN Discriminator;");
				
				await userManager.CreateAsync(new ApplicationUser { UserName = "ioniq_alex", Email = "ionica.alexandru@email.com" }, "123456");
				await userManager.CreateAsync(new ApplicationUser { UserName = "barbu_radu", Email = "barbu.radu@email.com" }, "qwerty");

				var members = new Member[] {
					new Member { FirstName = "Ionica", LastName = "Alexandru", Email = "ionica.alexandru@email.com"},
					new Member { FirstName = "Barbu", LastName = "Radu", Email = "barbu.radu@email.com"}
				};

				var tickets = new Ticket[]
				{
					new Ticket { Title = "Cumparaturi iarna", Status = TicketStatus.InProgress, Description = "Trebuie\nsa mergem\nla cumparaturi\npentru sarbatori!!!"},
					new Ticket { Title = "Reparatii frigider", Status = TicketStatus.Completed ,Description = "Trebuie dus frigiderul la service!\nURGENT!!!"}
				};

				dbContext.Members.AddRange(members);
				dbContext.Tickets.AddRange(tickets);

				tickets[0].Assignees = new Member[] { members[0], members[1] };
				tickets[1].Assignees = new Member[] { members[0] };

				tickets[0].Creator = members[0];
				tickets[1].Creator = members[1];

				dbContext.SaveChanges();
			}
		}

	}
}