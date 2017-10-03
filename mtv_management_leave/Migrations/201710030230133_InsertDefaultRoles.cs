namespace mtv_management_leave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertDefaultRoles : DbMigration
    {
        public override void Up()
        {
            this.Sql("delete from AspNetRoles");
            this.Sql("insert into AspNetRoles(Name) values('Super admin')");
            this.Sql("insert into AspNetRoles(Name) values('Admin')");
            this.Sql("insert into AspNetRoles(Name) values('User')");


        }

        public override void Down()
        {
            this.Sql("delete from AspNetRoles");
        }
    }
}
