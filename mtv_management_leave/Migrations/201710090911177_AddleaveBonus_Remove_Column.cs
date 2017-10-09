namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddleaveBonus_Remove_Column : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AddLeave", "LeaveTypeId", "dbo.MasterLeaveType");
            DropIndex("dbo.AddLeave", new[] { "LeaveTypeId" });
            RenameColumn(table: "dbo.AddLeave", name: "LeaveTypeId", newName: "MasterLeaveType_Id");
            AlterColumn("dbo.AddLeave", "MasterLeaveType_Id", c => c.Int());
            CreateIndex("dbo.AddLeave", "MasterLeaveType_Id");
            AddForeignKey("dbo.AddLeave", "MasterLeaveType_Id", "dbo.MasterLeaveType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AddLeave", "MasterLeaveType_Id", "dbo.MasterLeaveType");
            DropIndex("dbo.AddLeave", new[] { "MasterLeaveType_Id" });
            AlterColumn("dbo.AddLeave", "MasterLeaveType_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.AddLeave", name: "MasterLeaveType_Id", newName: "LeaveTypeId");
            CreateIndex("dbo.AddLeave", "LeaveTypeId");
            AddForeignKey("dbo.AddLeave", "LeaveTypeId", "dbo.MasterLeaveType", "Id", cascadeDelete: true);
        }
    }
}
