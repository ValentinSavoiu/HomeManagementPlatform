using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

				dbContext.Database.ExecuteSqlCommand("ALTER TABLE AspNetUsers DROP COLUMN Discriminator;");
				dbContext.Database.ExecuteSqlCommand("ALTER TABLE [dbo].[AspNetUsers] ADD [Discriminator] [nvarchar](128) DEFAULT 'ApplicationUser';");

				var users = new ApplicationUser[] { 
					new ApplicationUser { UserName = "ioniq_alex", Email = "ionica.alexandru@email.com" },
					new ApplicationUser { UserName = "barbu_radu", Email = "barbu.radu@email.com" },
					new ApplicationUser { UserName = "nicoleta.rebeca", Email = "nicoleta.rebeca@nice-email.com" },
					new ApplicationUser { UserName = "delia_mariana", Email = "delia_mariana@email.com" },
				};

				var passwords = new String[] { "123456", "qwerty", "123456", "qwerty"};

				for(int i = 0; i < users.Length; i++)
                {
					await userManager.CreateAsync(users[i], passwords[i]);
					dbContext.ApplicationUsers.Attach(users[i]);
				}

				var tickets = new Ticket[]
				{
					new Ticket { Title = "Cumparaturi iarna", Status = TicketStatus.InProgress, Description = "Trebuie\nsa mergem\nla cumparaturi\npentru sarbatori!!!"},
					new Ticket { Title = "Reparatii frigider", Status = TicketStatus.Completed ,Description = "Trebuie dus frigiderul la service!\nURGENT!!!"},
					new Ticket { Title = "Excursie", Status = TicketStatus.NotStarted, Description = "Niste intrebari:\n    Unde mergem?\n    Ce facem?\n    Cine mai vine?"},
				};

				dbContext.Tickets.AddRange(tickets);
				
				var groups = new Group[] { 
					new Group { Name = "Grup frumos", OwnerEmail = "ionica.alexandru@email.com" },
					new Group { Name = "grup respectat", OwnerEmail = "barbu.radu@email.com" },
					new Group { Name = "Eu sunt cineva", OwnerEmail = "nicoleta.rebeca@nice-email.com" },
				};

				dbContext.Groups.AddRange(groups);

				tickets[0].Assignees = new ApplicationUser[] { users[0], users[1] };
				tickets[1].Assignees = new ApplicationUser[] { users[0] };
				tickets[2].Assignees = new ApplicationUser[] { users[0], users[1], users[2], users[3] };

				tickets[0].Creator = users[0];
				tickets[1].Creator = users[1];
				tickets[2].Creator = users[2];

				tickets[0].Group = groups[0];
				tickets[1].Group = groups[1];
				tickets[2].Group = groups[2];

				var groupMembers = new GroupMember[]
				{
					new GroupMember{User = users[0], Group = groups[0], NickName = "Tata_sef"},
					new GroupMember{User = users[1], Group = groups[0], NickName = "Fiul_member"},
					new GroupMember{User = users[0], Group = groups[1], NickName = "Tata_membru"},
					new GroupMember{User = users[1], Group = groups[1], NickName = "Fiul_sef"},
					new GroupMember{User = users[2], Group = groups[2], NickName = "Mama_sef"},

				};

				dbContext.GroupMembers.AddRange(groupMembers);

				var comments = new List<Comment> { };
				comments.Add(new Comment { PreviousComment = null, Ticket = tickets[0], CommentCreator = users[0], Text = "Ce trebuie cumparat? :)\nCat buget avem pentru cumparaturi?\n", DateCreated = new DateTime(2022, 12, 20, 10, 30, 20) });
				comments.Add(new Comment { PreviousComment = comments[0], Ticket = tickets[0], CommentCreator = users[1], Text = "In primul rand trebuie luate ingrediente pentru cozonac:\n   Faina\n   Zahar\n   Drojdie\n", DateCreated = new DateTime(2022, 12, 20, 15, 15, 10) });
				comments.Add(new Comment { PreviousComment = comments[1], Ticket = tickets[0], CommentCreator = users[2], Text = "Legat de buget, cred ca avem destul cat sa ne permitem niste ingrediente de cozonac", DateCreated = new DateTime(2022, 12, 21, 9, 45, 31) });

				dbContext.Comments.AddRange(comments);

				dbContext.SaveChanges();
			}
		}

	}
}