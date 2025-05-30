using AdminDashboardMVC.Models;
using AdminDashboardMVC.Models.Employees;
using AdminDashboardMVC.Models.Sales;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AdminDashboard.Data
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        public DbInitializer(ApplicationDbContext context)
        {

        }

    }
}
