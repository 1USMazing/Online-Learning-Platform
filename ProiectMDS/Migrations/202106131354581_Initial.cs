namespace ProiectMDS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartines",
                c => new
                    {
                        idEchipa = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        Email = c.String(),
                        MembruId = c.String(nullable: false),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.idEchipa)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.SubjectId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(nullable: false),
                        SubjectDescription = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(),
                        Status = c.String(),
                        AssignTo = c.String(),
                        SubjectId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CourseId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
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
                        Subject_SubjectId = c.Int(),
                        Apartine_idEchipa = c.Int(),
                        ApartineT_idCourse = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .ForeignKey("dbo.Apartines", t => t.Apartine_idEchipa)
                .ForeignKey("dbo.ApartineTs", t => t.ApartineT_idCourse)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Subject_SubjectId)
                .Index(t => t.Apartine_idEchipa)
                .Index(t => t.ApartineT_idCourse);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApartineTs",
                c => new
                    {
                        idCourse = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Email = c.String(),
                        MembruId = c.String(nullable: false),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.idCourse)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.CourseId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "ApartineT_idCourse", "dbo.ApartineTs");
            DropForeignKey("dbo.ApartineTs", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApartineTs", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.AspNetUsers", "Apartine_idEchipa", "dbo.Apartines");
            DropForeignKey("dbo.Apartines", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Apartines", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Courses", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "CourseId", "dbo.Courses");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ApartineTs", new[] { "user_Id" });
            DropIndex("dbo.ApartineTs", new[] { "CourseId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "ApartineT_idCourse" });
            DropIndex("dbo.AspNetUsers", new[] { "Apartine_idEchipa" });
            DropIndex("dbo.AspNetUsers", new[] { "Subject_SubjectId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "CourseId" });
            DropIndex("dbo.Courses", new[] { "SubjectId" });
            DropIndex("dbo.Subjects", new[] { "UserId" });
            DropIndex("dbo.Apartines", new[] { "user_Id" });
            DropIndex("dbo.Apartines", new[] { "SubjectId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ApartineTs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Courses");
            DropTable("dbo.Subjects");
            DropTable("dbo.Apartines");
        }
    }
}
