using PagedList;
using PagedList.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using u22560646_HW3.Models;

namespace u22560646_HW3.Controllers
{
    public class MaintainController : Controller
    {
        private readonly BikeStoreContext db = new BikeStoreContext();

        public async Task<ActionResult> Index(
            int? brandId, int? categoryId,
            int staffPage = 1, int custPage = 1, int prodPage = 1)
        {
            int pageSize = 5;

            var vm = new HomeViewModel
            {
                Brands = await db.Brands.OrderBy(b => b.BrandName).ToListAsync(),
                Categories = await db.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                Stores = await db.Stores.OrderBy(s => s.StoreName).ToListAsync(),
                SelectedBrandId = brandId,
                SelectedCategoryId = categoryId
            };

            
            var staffQuery = db.Staffs.Include(s => s.Store).OrderBy(s => s.LastName);
            vm.PagedStaffs = await staffQuery.ToPagedListAsync(staffPage, pageSize);

            
            var custQuery = db.Customers.OrderBy(c => c.LastName);
            vm.PagedCustomers = await custQuery.ToPagedListAsync(custPage, pageSize);

            
            var prodQuery = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName)
                .AsQueryable();

            if (brandId.HasValue) prodQuery = prodQuery.Where(p => p.BrandId == brandId.Value);
            if (categoryId.HasValue) prodQuery = prodQuery.Where(p => p.CategoryId == categoryId.Value);

            vm.PagedProducts = await prodQuery.ToPagedListAsync(prodPage, pageSize);

            
            var staffIds = vm.PagedStaffs.Select(s => s.StaffId).ToList();
            var soldItems = await db.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.StaffId.HasValue && staffIds.Contains(oi.Order.StaffId.Value))
                .ToListAsync();

            vm.StaffSoldItems = soldItems
                .GroupBy(oi => oi.Order.StaffId.Value)
                .ToDictionary(g => g.Key, g => g.Take(3).ToList());

            var custIds = vm.PagedCustomers.Select(c => c.CustomerId).ToList();
            var purchasedItems = await db.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.CustomerId.HasValue && custIds.Contains(oi.Order.CustomerId.Value))
                .ToListAsync();

            vm.CustomerPurchasedItems = purchasedItems
                .GroupBy(oi => oi.Order.CustomerId.Value)
                .ToDictionary(g => g.Key, g => g.Take(3).ToList());

            
            ViewBag.StaffPage = staffPage;
            ViewBag.CustPage = custPage;
            ViewBag.ProdPage = prodPage;

            return View(vm);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(Staff staff) => await SaveStaff(staff, "added");

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStaff(Staff staff) => await SaveStaff(staff, "updated");

        private async Task<ActionResult> SaveStaff(Staff staff, string action)
        {
            if (ModelState.IsValid)
            {
                if (staff.StaffId == 0) db.Staffs.Add(staff);
                else db.Entry(staff).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Message"] = $"Staff {action}!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteStaff(int staffId)
        {
            var staff = await db.Staffs.FindAsync(staffId);
            if (staff != null)
            {
                db.Staffs.Remove(staff);
                await db.SaveChangesAsync();
                TempData["Message"] = "Staff deleted!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomer(Customer customer) => await SaveCustomer(customer, "added");

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCustomer(Customer customer) => await SaveCustomer(customer, "updated");

        private async Task<ActionResult> SaveCustomer(Customer customer, string action)
        {
            if (ModelState.IsValid)
            {
                if (customer.CustomerId == 0) db.Customers.Add(customer);
                else db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Message"] = $"Customer {action}!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            var cust = await db.Customers.FindAsync(customerId);
            if (cust != null)
            {
                db.Customers.Remove(cust);
                await db.SaveChangesAsync();
                TempData["Message"] = "Customer deleted!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProduct(Product product) => await SaveProduct(product, "added");

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(Product product) => await SaveProduct(product, "updated");

        private async Task<ActionResult> SaveProduct(Product product, string action)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductId == 0) db.Products.Add(product);
                else db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Message"] = $"Product {action}!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var prod = await db.Products.FindAsync(productId);
            if (prod != null)
            {
                db.Products.Remove(prod);
                await db.SaveChangesAsync();
                TempData["Message"] = "Product deleted!";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}