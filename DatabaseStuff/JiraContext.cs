using Microsoft.AspNet.Identity.EntityFramework;
using mss_project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace mss_project.DatabaseStuff
{
    public class JiraContext: IdentityDbContext
    {
        public JiraContext() : base("DefaultConnection")
        {
        }
        public DbSet<Member> Members { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            
        }

    }
}