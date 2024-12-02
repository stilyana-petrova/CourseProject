using ArtGallery.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Abstraction
{
    public interface IArtistService
    {
        List<Artist> GetArtists();
        Artist GetArtistById(int artistId);
        List<Product> GetProductsByArtist(int artistId);

        int CreateArtist(string name, int yearBorn, string biography, string picture);
        bool Update(int artistId, string name, int yearBorn, string biography, string picture) ;
        public bool RemoveById(int artistId);
    }
}
