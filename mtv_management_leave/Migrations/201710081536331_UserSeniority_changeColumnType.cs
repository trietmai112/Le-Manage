namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSeniority_changeColumnType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestChangeInout",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        Intime = c.DateTime(),
                        OutTime = c.DateTime(),
                        Date = c.DateTime(nullable: false),
                        status = c.Int(nullable: false),
                        Reason = c.String(),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uid, cascadeDelete: true)
                .Index(t => t.Uid);
            
            AlterColumn("dbo.UserSeniority", "Month1", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month2", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month3", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month4", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month5", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month6", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month7", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month8", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month9", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month10", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month11", c => c.Int());
            AlterColumn("dbo.UserSeniority", "Month12", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestChangeInout", "Uid", "dbo.AspNetUsers");
            DropIndex("dbo.RequestChangeInout", new[] { "Uid" });
            AlterColumn("dbo.UserSeniority", "Month12", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month11", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month10", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month9", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month8", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month7", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month6", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month5", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month4", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month3", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month2", c => c.Byte());
            AlterColumn("dbo.UserSeniority", "Month1", c => c.Byte());
            DropTable("dbo.RequestChangeInout");
        }
    }
}
