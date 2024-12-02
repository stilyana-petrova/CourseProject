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
    public class ArtistService:IArtistService
    {
        private readonly ApplicationDbContext _context;
        public ArtistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Artist> GetArtists()
        {
            List<Artist> artists = _context.Artists.ToList();
            return artists;
        }

        public Artist GetArtistById(int artistId)
        {
            return _context.Artists.Find(artistId);
        }

        public List<Product> GetProductsByArtist(int artistId)
        {
            return _context.Products.Where(p => p.ArtistId==artistId).ToList();
        }
        public int CreateArtist(string name, int yearBorn, string biography, string picture)
        {
            var artist = new Artist
            {
                Name=name,
                YearBorn = yearBorn,
                Biography = biography,
                Picture=picture
            };

            _context.Artists.Add(artist);
            _context.SaveChanges();

            return artist.Id;
        }

        public bool Update(int artistId, string name, int yearBorn, string biography, string picture)
        {
            var artist = GetArtistById(artistId);
            if (artist == default(Artist))
            {
                return false;
            }
            artist.Name = name;
            artist.YearBorn = yearBorn;
            artist.Biography = biography;
            artist.Picture = picture;

            _context.Update(artist);
            return _context.SaveChanges() != 0;
        }

        public bool RemoveById(int artistId)
        {
            var artist = GetArtistById(artistId);
            if (artist == default(Artist))
            {
                return false;
            }
            _context.Remove(artist);
            return _context.SaveChanges() != 0;
        }
    }
}
