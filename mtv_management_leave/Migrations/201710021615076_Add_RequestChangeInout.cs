namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RequestChangeInout : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestChangeInout", "Uid", "dbo.AspNetUsers");
            DropIndex("dbo.RequestChangeInout", new[] { "Uid" });
            DropTable("dbo.RequestChangeInout");
        }
    }
}
