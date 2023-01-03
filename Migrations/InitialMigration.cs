namespace mss_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserGroup",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Group_GroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Group_GroupID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.Group_GroupID, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Group_GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ticket", "Member_MemberID", "dbo.Member");
            DropForeignKey("dbo.Ticket", "Creator_MemberID", "dbo.Member");
            DropForeignKey("dbo.Ticket", "Assignee_MemberID", "dbo.Member");
            DropForeignKey("dbo.ApplicationUserGroup", "Group_GroupID", "dbo.Group");
            DropForeignKey("dbo.ApplicationUserGroup", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserGroup", new[] { "Group_GroupID" });
            DropIndex("dbo.ApplicationUserGroup", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ticket", new[] { "Member_MemberID" });
            DropIndex("dbo.Ticket", new[] { "Creator_MemberID" });
            DropIndex("dbo.Ticket", new[] { "Assignee_MemberID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.ApplicationUserGroup");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ticket");
            DropTable("dbo.Member");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Group");
        }
    }
}
