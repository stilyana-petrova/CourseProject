using ArtGallery.Models.Artist;
using ArtGallery.Models.Category;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models.Product
{
    public class ProductIndexVM
    {
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]

        public string Description { get; set; }
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } 

        [Display(Name = "Artist")]
        public int ArtistId { get; set; }
        public string ArtistName { get; set; } 
        [Display(Name = "Picture")]

        public string Picture { get; set; }

        // public Stock Stock { get; set; }

        //  [Range(0, 100)]
        public int Quantity { get; set; }
        [Display(Name = "Price")]

        public decimal Price { get; set; }
        [Display(Name = "Discount")]

        public decimal Discount { get; set; }
    }
}
