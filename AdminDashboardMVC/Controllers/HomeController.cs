using AdminDashboardMVC.Models;
using AdminDashboardMVC.Models.Sales;
using System;
using Kendo.Mvc.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using AdminDashboardMVC.Models.Employees;
using System.Security.Claims;
using AdminDashboardMVC.Models.UserSettings;

namespace AdminDashboardMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Performance()
        {

            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            PerformanceViewModel model = new PerformanceViewModel()
            {
                Target = sales
                    .Where(sale => sale.TransactionDate > new DateTime(2021, 6, 1))
                    .Where(sale => sale.TransactionDate < new DateTime(2021, 11, 1))
                    .Count(),
                TotalSalesPerWeekCount = sales
                    .Where(sale => sale.TransactionDate > new DateTime(2021, 6, 23))
                    .Where(sale => sale.TransactionDate < new DateTime(2021, 11, 1))
                    .Count(),
                LastMonthSalesCount = sales
                    .Where(sale => sale.TransactionDate > new DateTime(2021, 6, 30))
                    .Where(sale => sale.TransactionDate < new DateTime(2021, 10, 1))
                    .Count(),
                LastThreeMonthsSalesCount = sales
                    .Where(sale => sale.TransactionDate > new DateTime(2021, 6, 30))
                    .Where(sale => sale.TransactionDate < new DateTime(2021, 11, 1))
                    .Count()
            };

            return View(model);
        }

        public ActionResult GetPerformanceSales()
        {

            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            var salesByRegion = sales
                                 .Where(sale => sale.TransactionDate > new DateTime(2021, 6, 1))
                                 .Where(sale => sale.TransactionDate < new DateTime(2021, 11, 1))
                                .GroupBy(sale => new { sale.Region, sale.TransactionDate.Year, sale.TransactionDate.Month })
                                .Select(group => new SalesByDateViewModel
                                {
                                    Date = new DateTime(group.Key.Year, group.Key.Month, 1),
                                    Region = group.Key.Region,
                                    Sum = group.Sum(sale => sale.Amount)
                                });

            return Json(salesByRegion.ToList());
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("Settings")]
        [Authorize]
        public ActionResult Settings()
        {
            ClaimsPrincipal currentUser = (ClaimsPrincipal)User;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            UserDetailsModel userDetails = null;

            if (claimsIdentity != null)
            {
                userDetails = new UserDetailsModel()
                {
                    Username = claimsIdentity.Claims.First().Value,
                    Email = claimsIdentity.Claims.First().Value,
                    Nickname = claimsIdentity.Claims.Last().Value,
                    Phone = "112345678901",
                    Website = "https://www.telerik.com/",
                    WorkPhone = "112345678901",
                    Country = "1",
                };
            }

            return View(userDetails);
        }

        public ActionResult GetSales(int type)
        {
            var salesByRegion = new List<SalesByDateViewModel>();
            using (var db = new ApplicationDbContext())
            {
                salesByRegion = db.Sales
                                 .Where(sale => sale.TransactionDate > new DateTime(2021, 8, 1))
                                 .Where(sale => sale.TransactionDate < new DateTime(2021, 10, 1))
                                 .GroupBy(sale => new { sale.Region, sale.TransactionDate.Year, sale.TransactionDate.Month }).ToList()
                                 .Select(group => new SalesByDateViewModel
                                 {
                                     Date = new DateTime(group.Key.Year, group.Key.Month, 1),
                                     Region = group.Key.Region,
                                     Sum = type != 1 ? group.Sum(sale => sale.Amount) : group.Count()
                                 }).ToList();
            }

            return Json(salesByRegion);
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request, int team)
        {
            var employees = new List<Employee>();
            using (var db = new ApplicationDbContext())
            {
                employees = db.Employees.ToList();
            }

            if (team != 1)
            {
                employees = employees.Where(x => x.JobTitle.Contains("Engineer")).ToList();
            }

            var data = employees.Select(x => new EmployeeViewModel()
            {
                Id = x.Id,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                Rating = x.Rating,
                Budget = x.Budget
            }).ToList();

            return Json(data.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult GetSalesPreferences()
        {
            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            var salesByPaymentType = sales
                                .Where(sale => sale.TransactionDate > new DateTime(2021, 1, 7))
                                .ToList()
                                .GroupBy(sale => new { sale.PaymentType, sale.TransactionDate.Date, sale.TransactionDate.DayOfWeek })
                                .Select(group => new
                                 {
                                     DayOfWeek = group.Key.DayOfWeek,
                                     PaymentType = group.Key.PaymentType,
                                     Min = group.Where(sale => sale.PaymentType == group.Key.PaymentType).Min(sale => sale.Amount -1),
                                     Max = group.Where(sale => sale.PaymentType == group.Key.PaymentType).Max(sale => sale.Amount + 1)
                                 }).OrderBy(o => o.DayOfWeek);

            return Json(salesByPaymentType.ToList());
        }

        public ActionResult GetSalesPerRegion(string region)
        {

            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            var target = region == "EMEA" ? 1200 : 1400;
            var salesPerRegion = sales
                .Where(sale => region == "EMEA" ? sale.Region == region : sale.Region != region)
                .GroupBy(sale => new { sale.Country, sale.Region })
                .Select(group => new SalesPerRegionViewModel()
                {
                    Country = group.Key.Country,
                    Completed = group.Sum(sale => sale.Amount) / target * 100,
                    NotCompleted = (target - group.Sum(sale => sale.Amount)) / target * 100
                })
                .OrderByDescending(x => x.Completed)
                .Take(5)
                .ToList();

            var colors = new string[] { "#FF6358", "#FFD246", "#AA46BE", "#2D73F5", "#28B4C8" };
            for (int i = 0; i < salesPerRegion.Count(); i++)
            {
                salesPerRegion[i].Color = colors[i];
            }

            return Json(salesPerRegion.ToList());
        }

        public JsonResult GetSalesPerGroup()
        {
            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            var salesPerGroup = sales
                .GroupBy(sale => new { sale.ProductGroup })
                .Select(group => new
                {
                    Group = group.Key.ProductGroup,
                    Amount = group.Where(sale => sale.ProductGroup == group.Key.ProductGroup).Sum(sale => sale.Amount),
                    Explode = group.Key.ProductGroup == "Consumer Food" ? true : false
                });
            return Json(salesPerGroup.ToList());
        }

        public JsonResult PerformanceData()
        {
            var sales = new List<Sale>();
            using (var db = new ApplicationDbContext())
            {
                sales = db.Sales.ToList();
            }

            var totalSales = sales.Count();
            var performanceData = sales
                                .GroupBy(sale => new { sale.ProductGroup, sale.Region })
                                .Select(group => new
                                {
                                    Product = group.Key.ProductGroup,
                                    Region = group.Key.Region,
                                    Amount = sales.Where(x => x.ProductGroup == group.Key.ProductGroup && x.Region == group.Key.Region).Sum(x => x.Amount),
                                    SalesCount = sales.Where(x => x.ProductGroup == group.Key.ProductGroup && x.Region == group.Key.Region).Count(),
                                    Rate = sales.Where(x => x.ProductGroup == group.Key.ProductGroup && x.Region == group.Key.Region).Sum(x => x.Amount) / sales.Where(x => x.ProductGroup == group.Key.ProductGroup && x.Region == group.Key.Region).Count() * 100
                                });

            return Json(performanceData.ToList());
        }

        public JsonResult GetProducts()
        {
            var Titles = new string[] {
                "All-in-One Mac",
                "Colorful Future with SSG500",
                "The Prestige Series",
                "Designs into Websites",
                "Out of the Shadow",
                "Design Solutions",
                "Remote Life",
                "Through the Lens",
                "Wireless Culture",
                "Changing the Game",
                "Thin Tech",
                "Mobile Edu"
            };
            var Descriptions = new string[]
            {
                "An alternative solution for your business. Get our latest offer now and save up to $300 with the terrific All-in-One Mac laptop!",
                "Add some color to the dull and grey everyday life with our latest  laptop offer! SSG500 will bring your creative ideas to life!",
                "The SA325 Prestige Series deliver an excellent audio performance, combined with the best noise cancellation and a ton of additional features.",
                "Advance the web development process with Artificial Design Intelligence to deliver the best user experience directly through your website.",
                "This is the right place if you’re trying to reduce your Shadow IT risk. Explore our latest guide on how to protect and manage your software assets.",
                "This is the right place if you’re looking for a new laptop design. Spice up your environment with our fresh ideas for laptop skins, stickers, and sleeves.",
                "Smart phones of the future are coming our way! Get a glimpse of how they might develop and what to expect from their functionalities.",
                "Look forward to the future in a different light with AI. Enjoy better, faster, and more intuitive photos in the 3D world.",
                "Listen to your favorite genres with style! Our latest headphone design offers excellent wireless audio for a better music experience.",
                "The future of the headphone technology is wireless. Get to know the latest trends for fashionable designs and improvements in the headphone world.",
                "The changing technology and economy are driving a new world of mobile computing. Explore our super-thin designs with folding displays.",
                "Harness the nature of the mobile phone with our latest Mobile Edu design, created to assist you with your studies for a more informal approach to learning.",
            };
            var products = Enumerable.Range(1, 50).Select(x => new
            {
                ProductID = x,
                Title = Titles[x % 12],
                Description = Descriptions[x % 12]
            });

            return Json(products.ToList());
        }
    }
}