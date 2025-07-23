using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Belloro.Data;
using Belloro.Models;
using Belloro.Models.ViewModels;

namespace Belloro.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Dashboard verileri
                var model = new DashboardViewModel
                {
                    // İstatistikler
                    TotalProducts = await _context.Products.CountAsync(),
                    TotalCategories = await _context.Categories.CountAsync(),
                    TotalOrders = await _context.Orders.CountAsync(),

                    // Son siparişler
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

                    // En çok satan ürünler
                    TopSellingProducts = await _context.Products
                        .Include(p => p.Category)
                        .Where(p => p.SalesCount > 0)
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

                return View("~/Views/Admin/Dashboard.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dashboard yüklenirken hata oluştu: {ex.Message}";
                return View("~/Views/Admin/Dashboard.cshtml", new DashboardViewModel());
            }
        }
    }
}