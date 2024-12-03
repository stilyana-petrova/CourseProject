using ArtGallery.Core.Abstraction;
using ArtGallery.Models.Artist;
using ArtGallery.Models.Category;
using ArtGallery.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
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

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
