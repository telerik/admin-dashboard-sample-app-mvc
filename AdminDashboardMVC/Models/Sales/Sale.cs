using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminDashboardMVC.Models.Sales
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string StoreId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionId { get; set; }
        public string ProductGroup { get; set; }
        public string Sku { get; set; }
        public double Amount { get; set; }
        public int PromotionId { get; set; }
        public int CustomerInfo { get; set; }
        public string PaymentType { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string Group { get; set; }
        public bool Explode { get; set; }
        public int SalesCount { get; set; }
        public double Rate { get; set; }
        public string Product { get; set; }
    }
}