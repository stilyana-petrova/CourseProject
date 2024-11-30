using ArtGallery.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Abstraction
{
    public interface IProductService
    {
        bool Create(string name, string description, int categoryId, int artistId, string picture, int quantity, decimal price, decimal discount);
        bool Update(int productId, string name, string description, int categoryId, int artistId, string picture, int quantity, decimal price, decimal discount);
        List<Product> GetProducts();
        Product GetProductById(int productId);
        bool RemoveById(int productId);
        List<Product> GetProducts(string searchStringCategoryName, string searchStringArtistName, string searchStringProductName);
        //bool AddArtistToProduct(int productId, int artistId);
    }
}
