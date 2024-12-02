using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Infrastructure.Data.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
     
        [Required]
        [Range(1000, 2024)]
        public int YearBorn { get; set; }

        public string Biography { get; set; }
        public string Picture {  get; set; }

        public virtual IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
