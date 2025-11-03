using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace u22560646_HW3.Models
{
    public class BikeStoreContext : DbContext
    {
        public BikeStoreContext() : base("name=BikeStoreConnection")
        {
            // DB created by SQL script; prevent EF migrations from altering schema
            Database.SetInitializer<BikeStoreContext>(null);
        }

        // production schema
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }

        // sales schema
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Map production tables
            modelBuilder.Entity<Brand>().ToTable("brands", "production");
            modelBuilder.Entity<Category>().ToTable("categories", "production");
            modelBuilder.Entity<Product>().ToTable("products", "production");
            modelBuilder.Entity<Stock>().ToTable("stocks", "production");

            // Map sales tables
            modelBuilder.Entity<Customer>().ToTable("customers", "sales");
            modelBuilder.Entity<Store>().ToTable("stores", "sales");
            modelBuilder.Entity<Staff>().ToTable("staffs", "sales");
            modelBuilder.Entity<Order>().ToTable("orders", "sales");
            modelBuilder.Entity<OrderItem>().ToTable("order_items", "sales");

            // Brand
            modelBuilder.Entity<Brand>()
                .HasKey(b => b.BrandId)
                .Property(b => b.BrandId).HasColumnName("brand_id");
            modelBuilder.Entity<Brand>().Property(b => b.BrandName).HasColumnName("brand_name").IsRequired().HasMaxLength(100);

            // Category
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId)
                .Property(c => c.CategoryId).HasColumnName("category_id");
            modelBuilder.Entity<Category>().Property(c => c.CategoryName).HasColumnName("category_name").IsRequired().HasMaxLength(100);

            // Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId)
                .Property(p => p.ProductId).HasColumnName("product_id");
            modelBuilder.Entity<Product>().Property(p => p.ProductName).HasColumnName("product_name").IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Product>().Property(p => p.BrandId).HasColumnName("brand_id");
            modelBuilder.Entity<Product>().Property(p => p.CategoryId).HasColumnName("category_id");
            modelBuilder.Entity<Product>().Property(p => p.ModelYear).HasColumnName("model_year");
            modelBuilder.Entity<Product>().Property(p => p.ListPrice).HasColumnName("list_price").HasPrecision(10, 2);

            // Stock (composite key)
            modelBuilder.Entity<Stock>()
                .HasKey(s => new { s.StoreId, s.ProductId });
            modelBuilder.Entity<Stock>().Property(s => s.StoreId).HasColumnName("store_id");
            modelBuilder.Entity<Stock>().Property(s => s.ProductId).HasColumnName("product_id");
            modelBuilder.Entity<Stock>().Property(s => s.Quantity).HasColumnName("quantity");

            // Customer (map zip_code)
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerId)
                .Property(c => c.CustomerId).HasColumnName("customer_id");
            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Phone).HasColumnName("phone").HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasColumnName("email").HasMaxLength(255);
            modelBuilder.Entity<Customer>().Property(c => c.Street).HasColumnName("street").HasMaxLength(255);
            modelBuilder.Entity<Customer>().Property(c => c.City).HasColumnName("city").HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.State).HasColumnName("state").HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.ZipCode).HasColumnName("zip_code").HasMaxLength(20);

            // Store (map zip_code)
            modelBuilder.Entity<Store>()
                .HasKey(s => s.StoreId)
                .Property(s => s.StoreId).HasColumnName("store_id");
            modelBuilder.Entity<Store>().Property(s => s.StoreName).HasColumnName("store_name").HasMaxLength(150);
            modelBuilder.Entity<Store>().Property(s => s.Phone).HasColumnName("phone").HasMaxLength(50);
            modelBuilder.Entity<Store>().Property(s => s.Email).HasColumnName("email").HasMaxLength(255);
            modelBuilder.Entity<Store>().Property(s => s.Street).HasColumnName("street").HasMaxLength(255);
            modelBuilder.Entity<Store>().Property(s => s.City).HasColumnName("city").HasMaxLength(100);
            modelBuilder.Entity<Store>().Property(s => s.State).HasColumnName("state").HasMaxLength(100);
            modelBuilder.Entity<Store>().Property(s => s.ZipCode).HasColumnName("zip_code").HasMaxLength(20);
            

            // Staff
            modelBuilder.Entity<Staff>()
                .HasKey(s => s.StaffId)
                .Property(s => s.StaffId).HasColumnName("staff_id");
            modelBuilder.Entity<Staff>().Property(s => s.FirstName).HasColumnName("first_name").HasMaxLength(100);
            modelBuilder.Entity<Staff>().Property(s => s.LastName).HasColumnName("last_name").HasMaxLength(100);
            modelBuilder.Entity<Staff>().Property(s => s.Email).HasColumnName("email").HasMaxLength(255);
            modelBuilder.Entity<Staff>().Property(s => s.Phone).HasColumnName("phone").HasMaxLength(50);
            modelBuilder.Entity<Staff>().Property(s => s.Active).HasColumnName("active");
            modelBuilder.Entity<Staff>().Property(s => s.StoreId).HasColumnName("store_id");
            modelBuilder.Entity<Staff>().Property(s => s.ManagerId).HasColumnName("manager_id");

            // Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId)
                .Property(o => o.OrderId).HasColumnName("order_id");
            modelBuilder.Entity<Order>().Property(o => o.CustomerId).HasColumnName("customer_id");
            modelBuilder.Entity<Order>().Property(o => o.OrderStatus).HasColumnName("order_status");
            modelBuilder.Entity<Order>().Property(o => o.OrderDate).HasColumnName("order_date");
            modelBuilder.Entity<Order>().Property(o => o.RequiredDate).HasColumnName("required_date");
            modelBuilder.Entity<Order>().Property(o => o.ShippedDate).HasColumnName("shipped_date");
            modelBuilder.Entity<Order>().Property(o => o.StoreId).HasColumnName("store_id");
            modelBuilder.Entity<Order>().Property(o => o.StaffId).HasColumnName("staff_id");

            // OrderItem composite key
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });
            modelBuilder.Entity<OrderItem>().Property(oi => oi.OrderId).HasColumnName("order_id");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.ItemId).HasColumnName("item_id");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.ProductId).HasColumnName("product_id");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Quantity).HasColumnName("quantity");
            modelBuilder.Entity<OrderItem>().Property(oi => oi.ListPrice).HasColumnName("list_price").HasPrecision(10, 2);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Discount).HasColumnName("discount").HasPrecision(5, 2);

            // Relationships
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Staff>()
                .HasRequired(s => s.Store)
                .WithMany()
                .HasForeignKey(s => s.StoreId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}