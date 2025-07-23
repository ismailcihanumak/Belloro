using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Belloro.Data;
using Belloro.Models.ViewModels;

namespace Belloro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Dashboard verileri
                var dashboardData = new DashboardViewModel
                {
                    // Ýstatistikler
                    TotalProducts = await _context.Products.CountAsync(),
                    TotalCategories = await _context.Categories.CountAsync(),
                    TotalOrders = await _context.Orders.CountAsync(),

                    // SON SÝPARÝÞLER - Veritabanýndan çek
                    RecentOrders = await _context.Orders
                        .Include(o => o.Product)
                        .ThenInclude(p => p!.Category)
                        .OrderByDescending(o => o.OrderDate)
                        .Take(5)
                        .Select(o => new RecentOrderViewModel
                        {
                            OrderNumber = o.OrderNumber,
                            CustomerName = o.CustomerName,
                            ProductName = o.ProductName,
                            CategoryName = o.Product != null && o.Product.Category != null
                                         ? o.Product.Category.Name
                                         : "Kategorisiz",
                            Amount = o.Amount,
                            OrderDate = o.OrderDate,
                            Status = o.Status
                        })
                        .ToListAsync(),

                    // EN ÇOK SATAN ÜRÜNLER - SalesCount'a göre
                    TopSellingProducts = await _context.Products
                        .Include(p => p.Category)
                        .Where(p => p.SalesCount > 0) // Sadece satýþý olan ürünler
                        .OrderByDescending(p => p.SalesCount)
                        .Take(5)
                        .Select(p => new TopSellingProductViewModel
                        {
                            ProductName = p.Name,
                            CategoryName = p.Category != null ? p.Category.Name : "Kategorisiz",
                            SalesCount = p.SalesCount,
                            Price = p.Price,
                            ImageUrl = p.ImageUrl,
                            Description = p.Description,
                            IsFeatured = p.IsFeatured,
                            IsNew = p.IsNew
                        })
                        .ToListAsync()
                };

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dashboard yüklenirken hata oluþtu: {ex.Message}";
                return View(new DashboardViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}