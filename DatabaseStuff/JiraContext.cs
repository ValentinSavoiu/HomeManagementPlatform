using mss_project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace mss_project.DatabaseStuff
{
    public class JiraContext: DbContext
    {
        public JiraContext() : base("JiraConnection")
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            
        }
    }
}