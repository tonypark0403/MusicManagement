using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    [Authorize]
    public class ArtistController : Controller
    {
        Manager m = new Manager();
        // GET: Artist
        public ActionResult Index()
        {
            return View(m.ArtistGetAll());
        }

        // GET: Artist/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        // GET: Artist/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            var form = new ArtistAddForm();
            form.GenreList = new SelectList(items: m.GenreGetAll(), dataValueField: "Name", dataTextField: "Name");
            return View(form);
        }

        // POST: Artist/Create
        [Authorize(Roles = "Executive")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(ArtistAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.ArtistAdd(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/addAlbum")]
        public ActionResult AddAlbum(int? id)
        {
            var a = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

            if (a == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new ArtistAlbumAddForm();
                form.ArtistName = a.Name;
                form.ArtistId = a.Id;
                form.GenreList = new SelectList(items: m.GenreGetAll(), dataValueField: "Name", dataTextField: "Name");

                return View(form);
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("Artist/{id}/addAlbum")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddAlbum(ArtistAlbumAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.ArtistAlbumAdd(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", "Album", new { id = addedItem.Id });
            }
        }
        [Authorize(Roles = "Executive")]
        [Route("Artist/{id}/addMediaItem")]
        public ActionResult AddMediaItem(int? id)
        {
            var a = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

            if (a == null)
            {
                return HttpNotFound();
            }
            var form = new MediaItemAddForm();
            form.ArtistId = a.Id;
            form.ArtistName = a.Name;
            return View(form);
        }

        [Authorize(Roles = "Executive")]
        [Route("Artist/{id}/addMediaItem")]
        [HttpPost]
        public ActionResult AddMediaItem(MediaItemAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.MediaItemAdd(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }
        //        [Authorize(Roles = "Clerk")]
        //        [Route("Album/{id}/addTrack")]
        //        public ActionResult AddTrack(int? id)
        //        {
        //            var a = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());
        //            if (a == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            else
        //            {
        //                var form = new AlbumTrackAddForm();
        //                form.AlbumId = a.Id;
        //                form.AlbumName = a.Name;
        //                form.GenreList = new SelectList(items: m.GenreGetAll(), dataValueField: "Name", dataTextField: "Name");
        //                return View(form);
        //            }
        //        }
        //
        //        [Authorize(Roles = "Clerk")]
        //        [Route("Album/{id}/addTrack")]
        //        [HttpPost]
        //        public ActionResult AddTrack(AlbumTrackAdd newItem)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(newItem);
        //            }
        //
        //            var addedItem = m.AlbumTrackAdd(newItem);
        //
        //            if (addedItem == null)
        //            {
        //                return View(newItem);
        //            }
        //            else
        //            {
        //                return RedirectToAction("details", "Track", new { id = addedItem.Id });
        //            }
        //        }
    }
}
