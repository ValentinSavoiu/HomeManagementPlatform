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
				// Discriminator is a column that is automatically created whenever a model is a derived class
				// it is not required because only ApplicationUser inherits from IdentityUser
				dbContext.Database.ExecuteSqlCommand("ALTER TABLE AspNetUsers DROP COLUMN Discriminator;");
				
				await userManager.CreateAsync(new ApplicationUser { UserName = "ioniq_alex", Email = "ionica.alexandru@email.com" }, "123456");
				await userManager.CreateAsync(new ApplicationUser { UserName = "barbu_radu", Email = "barbu.radu@email.com" }, "qwerty");

				dbContext.Members.AddRange(new Member[] {
					new Member { MemberID = 1, FirstName = "Ionica", LastName = "Alexandru", Email = "ionica.alexandru@email.com"},
					new Member { MemberID = 2, FirstName = "Barbu", LastName = "Radu", Email = "barbu.radu@email.com"}
				});

				dbContext.SaveChanges();

				dbContext.Tickets.AddRange(new Ticket[]
				{
					new Ticket { Title = "Cumparaturi iarna", Description = "Trebuie\nsa mergem\nla cumparaturi\npentru sarbatori!!!", CreatorID = 1, AssigneeID = 2},
					new Ticket { Title = "Reparatii frigider", Description = "Trebuie dus frigiderul la service!\nURGENT!!!", CreatorID = 2, AssigneeID = 1}
				});

				dbContext.SaveChanges();
			}
		}

	}
}