namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TypeAndStatusAddLeave : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddLeave", "Type", c => c.String());
            AddColumn("dbo.AddLeave", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AddLeave", "Status");
            DropColumn("dbo.AddLeave", "Type");
        }
    }
}
