using ArtGallery.Core.Abstraction;
using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Artist;
using ArtGallery.Models.Category;
using ArtGallery.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize(Roles="Admin")]
    public class ProductController : Controller
    {
        private readonly IArtistService _artistService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ProductController(IArtistService artistService, ICategoryService categoryService, IProductService productService)
        {
            _artistService = artistService;
            _categoryService = categoryService;
            _productService = productService;
        }
        [AllowAnonymous]
        // GET: ProductController
        public ActionResult Index(string searchStringCategoryName, string searchStringArtistName, string searchStringProductName)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringArtistName, searchStringProductName)
                .Select(product => new ProductIndexVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    ArtistId = product.ArtistId,
                    ArtistName = product.Artist.Name,
                    Picture = product.Picture,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Discount = product.Discount
                }).ToList();
            return View(products);
        }
        [AllowAnonymous]
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            Product item=_productService.GetProductById(id);
            if (item == null) return NotFound();

            ProductIndexVM product = new ProductIndexVM()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                ArtistId = item.ArtistId,
                ArtistName = item.Artist.Name,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Artists = _artistService.GetArtists()
                .Select(x => new ArtistVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    YearBorn=x.YearBorn,
                    Biography=x.Biography,
                    Picture=x.Picture
                }).ToList();
            product.Categories=_categoryService.GetCategories()
                .Select(x=> new CategoryVM()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            return View(product);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createdId = _productService.Create(product.Name, product.Description, product.CategoryId, product.ArtistId, product.Picture, product.Quantity, product.Price, product.Discount);
                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            ProductCreateVM updatedProduct = new ProductCreateVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ArtistId = product.ArtistId,
                Picture = product.Picture,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };

            updatedProduct.Categories = _categoryService.GetCategories()
                .Select(c => new CategoryVM()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
            updatedProduct.Artists = _artistService.GetArtists()
                .Select(a => new ArtistVM()
                {
                    Id = a.Id,
                    Name = a.Name,
                    YearBorn = a.YearBorn,
                    Biography = a.Biography,
                    Picture = a.Picture
                }).ToList();

            return View(updatedProduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.Name, product.Description, product.CategoryId, product.ArtistId, product.Picture, product.Quantity, product.Price, product.Discount);
                if (updated) return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = _productService.GetProductById(id);
            if (item == null) return NotFound();

            ProductIndexVM product = new ProductIndexVM()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Name,
                ArtistId = item.ArtistId,
                ArtistName = item.Artist.Name,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
           var deleted=_productService.RemoveById(id);
            if(deleted) return RedirectToAction(nameof(Index));
            else return View();
        }
    }
}
