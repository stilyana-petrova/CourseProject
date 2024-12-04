using ArtGallery.Core.Abstraction;
using ArtGallery.Infrastructure.Data.Entities;
using ArtGallery.Models.Artist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ArtistController : Controller
    {
        private readonly IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }
        [AllowAnonymous]
        // GET: ArtistController
        public ActionResult Index()
        {
            var artists = _artistService.GetArtists()
                       .Select(a => new ArtistVM
                       {
                           Id = a.Id,
                           Name = a.Name,
                           YearBorn = a.YearBorn,
                           Biography = a.Biography,
                           Picture = a.Picture,
                       }).ToList();

            return View(artists);
        }
        [AllowAnonymous]
        // GET: ArtistController/Details/5
        public ActionResult Details(int id)
        {
            Artist artist = _artistService.GetArtistById(id);
            if (artist == null) return NotFound();

            ArtistVM updatedArtist = new ArtistVM()
            {
                Id = artist.Id,
                Name = artist.Name,
                YearBorn = artist.YearBorn,
                Biography = artist.Biography,
                Picture = artist.Picture
            };
            return View(updatedArtist);
        }

        // GET: ArtistController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ArtistVM artist)
        {
            if (ModelState.IsValid)
            {
                var success = _artistService.CreateArtist(artist.Name, artist.YearBorn, artist.Biography, artist.Picture);
                if (success > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to create artist.");
            }

            return View(artist);
        }

        // GET: ArtistController/Edit/5
        public ActionResult Edit(int id)
        {
            Artist artist = _artistService.GetArtistById(id);
            if (artist == null) return NotFound();

            ArtistVM updatedArtist = new ArtistVM()
            {
                Id = artist.Id,
                Name = artist.Name,
                YearBorn = artist.YearBorn,
                Biography = artist.Biography,
                Picture = artist.Picture
            };
            return View(updatedArtist);
        }

        // POST: ArtistController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ArtistVM artist)
        {
            if (ModelState.IsValid)
            {
                var updated = _artistService.Update(id, artist.Name, artist.YearBorn, artist.Biography, artist.Picture);
                if (updated) return RedirectToAction(nameof(Index));
            }

            return View(artist);
        }


        // GET: ArtistController/Delete/5
        public ActionResult Delete(int id)
        {
            Artist item = _artistService.GetArtistById(id);
            if (item == null) return NotFound();

            ArtistVM artist = new ArtistVM()
            {
                Id = item.Id,
                Name = item.Name,
                YearBorn = item.YearBorn,
                Biography = item.Biography,
                Picture = item.Picture
            };
            return View(artist);
        }

        // POST: ArtistController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted=_artistService.RemoveById(id);
            if (deleted) return RedirectToAction(nameof(Index));
            else return View();
        }
    }
}
