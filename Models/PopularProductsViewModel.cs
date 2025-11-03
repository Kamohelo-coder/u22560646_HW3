using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class PopularProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }

    public class PopularProductsViewModel
    {
        public List<PopularProductItem> Items { get; set; }
        public List<string> SavedReports { get; set; }
    }
}