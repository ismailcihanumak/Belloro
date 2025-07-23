using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Belloro.Data;
using Belloro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Belloro.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Ürün listesi
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["CurrentSort"] = sortOrder;

            ViewBag.Categories = await _context.Categories.ToListAsync();

            var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

            productsQuery = sortOrder switch
            {
                "name_desc" => productsQuery.OrderByDescending(p => p.Name),
                "price" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                _ => productsQuery.OrderBy(p => p.Name),
            };

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        // Detay için Session ID yaz ve /Details'a yönlendir
        public IActionResult SetDetailsSession(int id)
        {
            HttpContext.Session.SetInt32("DetailsProductId", id);
            return RedirectToAction("Details");
        }

        // Ürün detayları (Session ile)
        public async Task<IActionResult> Details()
        {
            var productId = HttpContext.Session.GetInt32("DetailsProductId");
            if (productId == null)
                return RedirectToAction(nameof(Index));

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == productId);

            if (product == null)
            {
                HttpContext.Session.Remove("DetailsProductId");
                TempData["Error"] = "Ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // Yeni ürün formu
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = await _context.Categories.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToListAsync();

            return View();
        }

        // Yeni ürün kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CategoryId,Description,ImageUrl,IsFeatured,IsNew")] Product product, IFormFile? ImageFile)
        {
            product.ProductId = 0;
            product.SalesCount = 0;
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(ImageFile.FileName);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        product.ImageUrl = "/assets/img/" + uniqueFileName;
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Ürün başarıyla eklendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["Error"] = "Kayıt sırasında bir hata oluştu.";
                }
            }

            ViewBag.CategoryId = await _context.Categories.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name,
                Selected = c.CategoryId == product.CategoryId
            }).ToListAsync();

            return View(product);
        }

        // Edit için Session yaz ve /Edit'e yönlendir
        public IActionResult SetEditSession(int id)
        {
            HttpContext.Session.SetInt32("EditProductId", id);
            return RedirectToAction("Edit");
        }

        // Ürün düzenle formu (Session ile)
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var productId = HttpContext.Session.GetInt32("EditProductId");

            if (productId == null)
                return RedirectToAction(nameof(Index));

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProductId == productId);

            if (product == null)
            {
                HttpContext.Session.Remove("EditProductId");
                TempData["Error"] = "Ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = await _context.Categories.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name,
                Selected = c.CategoryId == product.CategoryId
            }).ToListAsync();

            return View(product);
        }

        // Ürün düzenle post (Session ile)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductId,Name,Price,CategoryId,SalesCount,Description,ImageUrl,IsFeatured,IsNew")] Product product, IFormFile? ImageFile)
        {
            var productId = HttpContext.Session.GetInt32("EditProductId");

            if (productId == null)
                return RedirectToAction(nameof(Index));

            product.ProductId = productId.Value;
            ModelState.Remove("Category");

            try
            {
                var existingProduct = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (existingProduct == null)
                {
                    HttpContext.Session.Remove("EditProductId");
                    TempData["Error"] = "Ürün bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl) && existingProduct.ImageUrl.StartsWith("/assets/"))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            try { System.IO.File.Delete(oldImagePath); } catch { }
                        }
                    }

                    product.ImageUrl = "/assets/img/" + uniqueFileName;
                }
                else if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }

                ModelState.Clear();

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("EditProductId");
                TempData["Success"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Güncelleme sırasında bir hata oluştu.";
                ViewBag.CategoryId = await _context.Categories.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == product.CategoryId
                }).ToListAsync();
                return View(product);
            }
        }

        // Ürün silme (POST ile silme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                TempData["Error"] = "Ürün bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Sipariş ilişkisi kontrolü
            bool hasOrders = await _context.Orders.AnyAsync(o => o.ProductId == id);
            if (hasOrders)
            {
                TempData["Error"] = "Bu ürüne ait siparişler olduğu için ürün silinemez!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Resim dosyası silme işlemi
                if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl.StartsWith("/assets/"))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        try { System.IO.File.Delete(imagePath); } catch { }
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ürün başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                TempData["Error"] = $"Silme sırasında bir hata oluştu: {innerExceptionMessage}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}