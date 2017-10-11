namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreColumnInTableRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "IsShow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "IsShow");
        }
    }
}
