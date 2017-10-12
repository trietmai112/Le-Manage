namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TurnbackColumnNameRawData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataInOutRaw", "Uid", c => c.Int(nullable: false));
            DropColumn("dbo.DataInOutRaw", "FPid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataInOutRaw", "FPid", c => c.Int(nullable: false));
            DropColumn("dbo.DataInOutRaw", "Uid");
        }
    }
}
