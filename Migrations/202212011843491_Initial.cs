namespace mss_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.MemberID);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        CreatorID = c.Int(nullable: false),
                        AssigneeID = c.Int(nullable: false),
                        Assignee_MemberID = c.Int(),
                        Creator_MemberID = c.Int(),
                        Member_MemberID = c.Int(),
                    })
                .PrimaryKey(t => t.TicketID)
                .ForeignKey("dbo.Member", t => t.Assignee_MemberID)
                .ForeignKey("dbo.Member", t => t.Creator_MemberID)
                .ForeignKey("dbo.Member", t => t.Member_MemberID)
                .Index(t => t.Assignee_MemberID)
                .Index(t => t.Creator_MemberID)
                .Index(t => t.Member_MemberID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "Member_MemberID", "dbo.Member");
            DropForeignKey("dbo.Ticket", "Creator_MemberID", "dbo.Member");
            DropForeignKey("dbo.Ticket", "Assignee_MemberID", "dbo.Member");
            DropIndex("dbo.Ticket", new[] { "Member_MemberID" });
            DropIndex("dbo.Ticket", new[] { "Creator_MemberID" });
            DropIndex("dbo.Ticket", new[] { "Assignee_MemberID" });
            DropTable("dbo.Ticket");
            DropTable("dbo.Member");
        }
    }
}
