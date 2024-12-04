using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ArtGallery.Infrastructure.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        [Required]
        public string Picture { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual List<OrderDetail> OrderDetail { get; set; }
        public virtual List<CartDetail> CartDetail { get; set; }

      //  [Range(0, 100)]
        public int Quantity { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be positive number.")]
        public decimal Price { get; set; }
        [Required]
        public decimal Discount { get; set; }

    }
}
