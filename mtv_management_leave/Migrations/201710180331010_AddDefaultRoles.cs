namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultRoles : DbMigration
    {
        public override void Up()
        {
            this.Sql("delete from AspNetRoles");
            this.Sql("insert into AspNetRoles(Name, IsShow) values('Super admin', 0)");
            this.Sql("insert into AspNetRoles(Name, IsShow) values('Admin', 1)");
            this.Sql("insert into AspNetRoles(Name, IsShow) values('User', 1)");


        }

        public override void Down()
        {
            this.Sql("delete from AspNetRoles");
        }
    }
}
