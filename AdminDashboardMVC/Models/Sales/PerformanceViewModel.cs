using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminDashboardMVC.Models.Sales
{
    public class PerformanceViewModel
    {
        public int Target { get; set; }
        public int TotalSalesPerWeekCount { get; set; }
        public int LastMonthSalesCount { get; set; }
        public int LastThreeMonthsSalesCount { get; set; }
    }
}