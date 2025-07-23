using Microsoft.EntityFrameworkCore;
using Belloro.Models;

namespace Belloro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Amount)
                .HasColumnType("decimal(18,2)");

            // DÜZELTİLMİŞ ilişki:
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products) // DOĞRU!
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data - Kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Lüks Saatler" },
                new Category { CategoryId = 2, Name = "Klasik Saatler" },
                new Category { CategoryId = 3, Name = "Spor Saatler" },
                new Category { CategoryId = 4, Name = "Premium Saatler" }
            );

            // Seed data - Ürünler
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Belloro Classic Gold",
                    Description = "Klasik altın kaplama lüks saat",
                    Price = 3450m,
                    CategoryId = 1,
                    ImageUrl = "/images/products/classic-gold.jpg",
                    IsFeatured = true,
                    IsNew = false,
                    SalesCount = 25
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Belloro Rose Gold",
                    Description = "Zarif rose gold kadın saati",
                    Price = 2780m,
                    CategoryId = 1,
                    ImageUrl = "/images/products/rose-gold.jpg",
                    IsFeatured = true,
                    IsNew = true,
                    SalesCount = 18
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Belloro Sport",
                    Description = "Su geçirmez spor saati",
                    Price = 1950m,
                    CategoryId = 3,
                    ImageUrl = "/images/products/sport.jpg",
                    IsFeatured = false,
                    IsNew = true,
                    SalesCount = 32
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Belloro Minimalist",
                    Description = "Minimal tasarım klasik saat",
                    Price = 2100m,
                    CategoryId = 2,
                    ImageUrl = "/images/products/minimalist.jpg",
                    IsFeatured = false,
                    IsNew = false,
                    SalesCount = 15
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Belloro Automatic",
                    Description = "Otomatik mekanizmalı premium saat",
                    Price = 4280m,
                    CategoryId = 4,
                    ImageUrl = "/images/products/automatic.jpg",
                    IsFeatured = true,
                    IsNew = false,
                    SalesCount = 8
                }
            );

            // Seed data - Siparişler
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    OrderNumber = "ORD-0025",
                    CustomerName = "Ahmet Yılmaz",
                    ProductName = "Belloro Classic Gold",
                    ProductId = 1,
                    Amount = 3450m,
                    Status = "Tamamlandı",
                    OrderDate = new DateTime(2025, 7, 19, 10, 30, 0)
                },
                new Order
                {
                    OrderId = 2,
                    OrderNumber = "ORD-0024",
                    CustomerName = "Zeynep Aydın",
                    ProductName = "Belloro Rose Gold",
                    ProductId = 2,
                    Amount = 2780m,
                    Status = "İşlemde",
                    OrderDate = new DateTime(2025, 7, 20, 14, 15, 0)
                },
                new Order
                {
                    OrderId = 3,
                    OrderNumber = "ORD-0023",
                    CustomerName = "Mehmet Kaya",
                    ProductName = "Belloro Sport",
                    ProductId = 3,
                    Amount = 1950m,
                    Status = "Kargoda",
                    OrderDate = new DateTime(2025, 7, 18, 16, 45, 0)
                },
                new Order
                {
                    OrderId = 4,
                    OrderNumber = "ORD-0022",
                    CustomerName = "Ayşe Demir",
                    ProductName = "Belloro Minimalist",
                    ProductId = 4,
                    Amount = 2100m,
                    Status = "Tamamlandı",
                    OrderDate = new DateTime(2025, 7, 16, 11, 20, 0)
                },
                new Order
                {
                    OrderId = 5,
                    OrderNumber = "ORD-0021",
                    CustomerName = "Can Yıldız",
                    ProductName = "Belloro Automatic",
                    ProductId = 5,
                    Amount = 4280m,
                    Status = "Tamamlandı",
                    OrderDate = new DateTime(2025, 7, 14, 9, 10, 0)
                }
            );
        }
    }
}