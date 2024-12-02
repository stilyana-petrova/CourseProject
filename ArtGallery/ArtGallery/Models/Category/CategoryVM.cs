using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models.Category
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Display(Name = "Category")]
        public string Name { get; set; }
    }
}
