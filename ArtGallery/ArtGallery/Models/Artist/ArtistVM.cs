using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models.Artist
{
    public class ArtistVM
    {
        public int Id { get; set; }

        [Display(Name = "Name of the artist")]
        public string Name { get; set; }

        [Required]
        [Range(1000, 2024)]
        [Display(Name = "Birth Year")]

        public int YearBorn { get; set; }

        [Display(Name = "Biography")]
        public string Biography { get; set; }
        [Display(Name = "Picture")]

        public string Picture { get; set; }

    }
}
