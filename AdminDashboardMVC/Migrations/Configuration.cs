namespace AdminDashboardMVC.Migrations
{
    using AdminDashboard.Data;
    using AdminDashboardMVC.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AdminDashboardMVC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AdminDashboardMVC.Models.ApplicationDbContext";
        }

        protected override async void Seed(AdminDashboardMVC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //await DbInitializer.Initialize(context);
        }
    }
}
