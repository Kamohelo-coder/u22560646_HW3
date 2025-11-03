using System;
using System.Collections.Generic;
using System.Web;
using PagedList; // Add this

namespace u22560646_HW3.Models
{
    public class HomeViewModel
    {
        // For Maintain page (full lists)
        public List<Staff> Staffs { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }

        public IPagedList<Staff> PagedStaffs { get; set; }
        public IPagedList<Customer> PagedCustomers { get; set; }
        public IPagedList<Product> PagedProducts { get; set; }

        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Store> Stores { get; set; }

        // For filtering
        public int? SelectedBrandId { get; set; }
        public int? SelectedCategoryId { get; set; }

        // For Sold/Purchased items
        public Dictionary<int, List<OrderItem>> StaffSoldItems { get; set; }
        public Dictionary<int, List<OrderItem>> CustomerPurchasedItems { get; set; }
    }

}