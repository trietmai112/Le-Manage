namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddLeave",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        LeaveTypeId = c.Int(nullable: false),
                        AddLeaveHour = c.Double(),
                        DateAdd = c.DateTime(),
                        Reason = c.String(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterLeaveType", t => t.LeaveTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid)
                .Index(t => t.LeaveTypeId);
            
            CreateTable(
                "dbo.MasterLeaveType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsPaidLeave = c.Boolean(),
                        IsBussinessLeave = c.Boolean(),
                        LeaveCode = c.String(),
                        Description = c.String(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegisterLeave",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        LeaveTypeId = c.Int(nullable: false),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        RegisterHour = c.Double(),
                        Reason = c.String(),
                        DateRegister = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        UserApprove = c.Int(),
                        DateApprove = c.DateTime(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterLeaveType", t => t.LeaveTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid)
                .Index(t => t.LeaveTypeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        DateOfBirth = c.DateTime(),
                        UserPermission = c.Byte(nullable: false),
                        DateBeginWork = c.DateTime(),
                        DateBeginProbation = c.DateTime(),
                        DateResign = c.DateTime(),
                        Password = c.String(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
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
                        UserId = c.Int(nullable: false),
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
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.DataBeginYear",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        AnnualLeave = c.Double(nullable: false),
                        DateBegin = c.DateTime(nullable: false),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid);
            
            CreateTable(
                "dbo.DataInOutRaw",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InOut",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        Intime = c.DateTime(nullable: false),
                        OutTime = c.DateTime(),
                        Date = c.DateTime(nullable: false),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid);
            
            CreateTable(
                "dbo.LeaveMonthly",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        Month = c.DateTime(nullable: false),
                        LeaveAvailable = c.Boolean(),
                        LeaveUsed = c.Boolean(),
                        LeaveRemain = c.Boolean(),
                        LeaveNonPaid = c.Boolean(),
                        IsMaterityLeave = c.Boolean(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid);
            
            CreateTable(
                "dbo.MasterLeaveDayCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserSeniority",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        Year = c.Int(),
                        Month1 = c.Byte(),
                        Month2 = c.Byte(),
                        Month3 = c.Byte(),
                        Month4 = c.Byte(),
                        Month5 = c.Byte(),
                        Month6 = c.Byte(),
                        Month7 = c.Byte(),
                        Month8 = c.Byte(),
                        Month9 = c.Byte(),
                        Month10 = c.Byte(),
                        Month11 = c.Byte(),
                        Month12 = c.Byte(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSeniority", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LeaveMonthly", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.InOut", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.DataBeginYear", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AddLeave", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.RegisterLeave", "Uid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.RegisterLeave", "LeaveTypeId", "dbo.MasterLeaveType");
            DropForeignKey("dbo.AddLeave", "LeaveTypeId", "dbo.MasterLeaveType");
            DropIndex("dbo.UserSeniority", new[] { "Uid" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LeaveMonthly", new[] { "Uid" });
            DropIndex("dbo.InOut", new[] { "Uid" });
            DropIndex("dbo.DataBeginYear", new[] { "Uid" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.RegisterLeave", new[] { "LeaveTypeId" });
            DropIndex("dbo.RegisterLeave", new[] { "Uid" });
            DropIndex("dbo.AddLeave", new[] { "LeaveTypeId" });
            DropIndex("dbo.AddLeave", new[] { "Uid" });
            DropTable("dbo.UserSeniority");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MasterLeaveDayCompany");
            DropTable("dbo.LeaveMonthly");
            DropTable("dbo.InOut");
            DropTable("dbo.DataInOutRaw");
            DropTable("dbo.DataBeginYear");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RegisterLeave");
            DropTable("dbo.MasterLeaveType");
            DropTable("dbo.AddLeave");
        }
    }
}
