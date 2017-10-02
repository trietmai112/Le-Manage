namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTableEmployeeInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeInfo",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FPId = c.Int(nullable: false),
                        UserCreated = c.Int(nullable: false),
                        UserUpdated = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeInfo");
        }
    }
}
