using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using u22560646_HW3.Models;
using PagedList;
using PagedList.EntityFramework;

namespace u22560646_HW3.Controllers
{
    public class HomeController : Controller
    {
        private readonly BikeStoreContext db = new BikeStoreContext();

        public async Task<ActionResult> Index(
            int? brandId, int? categoryId,
            int staffPage = 1, int custPage = 1, int prodPage = 1)
        {
            int pageSize = 5;

            // Load dropdown data
            var brands = await db.Brands.OrderBy(b => b.BrandName).ToListAsync();
            var categories = await db.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            var stores = await db.Stores.OrderBy(s => s.StoreName).ToListAsync();

            // Base queries
            var staffQuery = db.Staffs.Include(s => s.Store).OrderBy(s => s.LastName);
            var custQuery = db.Customers.OrderBy(c => c.LastName);
            var prodQuery = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName);

            // Apply filters
            if (brandId.HasValue)
                prodQuery = (IOrderedQueryable<Product>)prodQuery.Where(p => p.BrandId == brandId.Value);
            if (categoryId.HasValue)
                prodQuery = (IOrderedQueryable<Product>)prodQuery.Where(p => p.CategoryId == categoryId.Value);

            var vm = new HomeViewModel
            {
                Brands = brands,
                Categories = categories,
                Stores = stores,

                PagedStaffs = await staffQuery.ToPagedListAsync(staffPage, pageSize),
                PagedCustomers = await custQuery.ToPagedListAsync(custPage, pageSize),
                PagedProducts = await prodQuery.ToPagedListAsync(prodPage, pageSize),

                SelectedBrandId = brandId,
                SelectedCategoryId = categoryId
            };

            // Preserve page numbers
            ViewBag.StaffPage = staffPage;
            ViewBag.CustPage = custPage;
            ViewBag.ProdPage = prodPage;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
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