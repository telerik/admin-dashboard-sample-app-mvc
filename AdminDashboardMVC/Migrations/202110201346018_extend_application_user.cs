namespace AdminDashboardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extend_application_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Company", c => c.String());
            AddColumn("dbo.AspNetUsers", "AgreeToTerms", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AgreeToTerms");
            DropColumn("dbo.AspNetUsers", "Company");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
    }
}
