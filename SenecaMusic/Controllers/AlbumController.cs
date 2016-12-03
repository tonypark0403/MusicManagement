using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        Manager m = new Manager();
        // GET: Album
        public ActionResult Index()
        {
            return View(m.AlbumGetAll());
        }

        // GET: Album/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }
        [Authorize(Roles = "Clerk")]
        [Route("Album/{id}/addTrack")]
        public ActionResult AddTrack(int? id)
        {
            var a = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());
            if (a == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new AlbumTrackAddForm();
                form.AlbumId = a.Id;
                form.AlbumName = a.Name;
                form.GenreList = new SelectList(items: m.GenreGetAll(), dataValueField: "Name", dataTextField: "Name");
                return View(form);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("Album/{id}/addTrack")]
        [HttpPost]
        public ActionResult AddTrack(AlbumTrackAdd newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            var addedItem = m.AlbumTrackAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", "Track", new { id = addedItem.Id });
            }
        }
    }
}
