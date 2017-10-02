namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_LeaveMonthly : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeaveMonthly", "LeaveAvailable", c => c.Double());
            AlterColumn("dbo.LeaveMonthly", "LeaveUsed", c => c.Double());
            AlterColumn("dbo.LeaveMonthly", "LeaveRemain", c => c.Double());
            AlterColumn("dbo.LeaveMonthly", "LeaveNonPaid", c => c.Double());
            DropColumn("dbo.LeaveMonthly", "IsMaterityLeave");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveMonthly", "IsMaterityLeave", c => c.Boolean());
            AlterColumn("dbo.LeaveMonthly", "LeaveNonPaid", c => c.Boolean());
            AlterColumn("dbo.LeaveMonthly", "LeaveRemain", c => c.Boolean());
            AlterColumn("dbo.LeaveMonthly", "LeaveUsed", c => c.Boolean());
            AlterColumn("dbo.LeaveMonthly", "LeaveAvailable", c => c.Boolean());
        }
    }
}
