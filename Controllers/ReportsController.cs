using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using u22560646_HW3.Models;


namespace u22560646_HW3.Controllers
{
    public class ReportsController : Controller
    {
        private readonly BikeStoreContext db = new BikeStoreContext();
        private readonly string reportsPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Reports");

        public ReportsController()
        {
            if (!Directory.Exists(reportsPath))
                Directory.CreateDirectory(reportsPath);
        }

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            // Popular products: count total quantity sold per product (order_items.quantity)
            var popular = await db.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new PopularProductItem
                {
                    ProductId = g.Key,
                    ProductName = db.Products.FirstOrDefault(p => p.ProductId == g.Key).ProductName,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(10)
                .ToListAsync();

            var savedReports = Directory.GetFiles(reportsPath).Select(Path.GetFileName).ToList();

            var vm = new PopularProductsViewModel
            {
                Items = popular,
                SavedReports = savedReports
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveReport(string fileName, string fileFormat)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                TempData["Error"] = "Enter a file name.";
                return RedirectToAction("Index");
            }

            var popular = await db.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductName = db.Products.FirstOrDefault(p => p.ProductId == g.Key).ProductName,
                    QuantitySold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(10)
                .ToListAsync();

            string fullPath = Path.Combine(reportsPath, $"{fileName}.{fileFormat}");
            using (var sw = new StreamWriter(fullPath))
            {
                if (fileFormat.ToLower() == "csv")
                {
                    sw.WriteLine("Product,QuantitySold");
                    foreach (var p in popular) sw.WriteLine($"{p.ProductName},{p.QuantitySold}");
                }
                else
                {
                    // simple plain text format
                    sw.WriteLine("Popular Products Report");
                    sw.WriteLine("-----------------------");
                    foreach (var p in popular) sw.WriteLine($"{p.ProductName} : {p.QuantitySold}");
                }
            }

            TempData["Message"] = $"Report saved as {fileName}.{fileFormat}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string name)
        {
            if (string.IsNullOrEmpty(name)) return RedirectToAction("Index");
            string fullPath = Path.Combine(reportsPath, name);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                TempData["Message"] = $"{name} deleted.";
            }
            return RedirectToAction("Index");
        }

        public FileResult Download(string name)
        {
            string fullPath = Path.Combine(reportsPath, name);
            if (!System.IO.File.Exists(fullPath)) throw new FileNotFoundException();

            string mime = name.EndsWith(".csv") ? "text/csv" : "text/plain";
            return File(fullPath, mime, name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}