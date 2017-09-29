namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInfo_ChangePassword_PasswordResetToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PasswordResetToken", c => c.String());
            DropColumn("dbo.AspNetUsers", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
            DropColumn("dbo.AspNetUsers", "PasswordResetToken");
        }
    }
}
