using ArtGallery.Models.Artist;
using ArtGallery.Models.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models.Product
{
    public class ProductCreateVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Product Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]

        public string Description { get; set; }
        [Required]
        [Display(Name = "Category")]

        public int CategoryId { get; set; }
        public virtual List<CategoryVM> Categories { get; set; } = new List<CategoryVM>();

        [Required]
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }
        public virtual List<ArtistVM> Artists { get; set; } = new List<ArtistVM>();
        [Required]
        [Display(Name = "Picture")]

        public string Picture { get; set; }

       // public Stock Stock { get; set; }

        //  [Range(0, 100)]
       public int Quantity { get; set; }
        [Required]
        [Display(Name = "Price")]

        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be positive number.")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Discount")]

        public decimal Discount { get; set; }
    }
}
