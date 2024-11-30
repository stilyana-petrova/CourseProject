using ArtGallery.Core.Abstraction;
using ArtGallery.Data;
using ArtGallery.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class ProductService:IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, string description, int categoryId, int artistId, string picture, int quantity, decimal price, decimal discount)
        {
            Product item = new Product
            {
                Name = name,
                Description = description,
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount
            };
            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public List<Product> GetProducts(string searchStringCategoryName, string searchStringArtistName, string searchStringProductName)
        {
            List<Product> products = _context.Products.ToList();
            if (!String.IsNullOrEmpty(searchStringCategoryName) && !String.IsNullOrEmpty(searchStringArtistName))
            {
                products = products.Where(x => x.Category.Name.ToLower().Contains(searchStringCategoryName.ToLower()) && x.Artist.Name.ToLower().Contains(searchStringArtistName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
                products = products.Where(x => x.Category.Name.ToLower().Contains(searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringArtistName))
            {
                products = products.Where(x => x.Artist.Name.ToLower().Contains(searchStringArtistName.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(searchStringProductName))
            {
                products = products.Where(x => x.Name.ToLower().Contains(searchStringProductName.ToLower())).ToList();
            }
            return products;
        }

        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int productId, string name, string description, int categoryId, int artistId, string picture, int quantity, decimal price, decimal discount)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }
            product.Name = name;
            product.Description = description;
            product.CategoryId = categoryId;
            product.Category = _context.Categories.Find(categoryId);
            product.Picture = picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;

            _context.Update(product);
            return _context.SaveChanges() != 0;
        }

       
    }
}
