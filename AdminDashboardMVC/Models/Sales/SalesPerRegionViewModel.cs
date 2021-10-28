using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminDashboardMVC.Models.Sales
{
    public class SalesPerRegionViewModel
    {
        public string Country { get; set; }
        public string Color { get; set; }
        public double Completed { get; set; }
        public double NotCompleted { get; set; }
    }
}