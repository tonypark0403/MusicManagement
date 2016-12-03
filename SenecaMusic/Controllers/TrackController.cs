using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    [Authorize]
    public class TrackController : Controller
    {
        Manager m = new Manager();
        // GET: Track
        public ActionResult Index()
        {
            return View(m.TrackGetAll());
        }

        // GET: Track/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.TrackGetById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(o);
            }
        }

        //        public ActionResult Details(int? id)
        //        {
        //            var o = m.TrackGetByIdWithDetail(id.GetValueOrDefault());
        //            if (o == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            else
        //            {
        //                return View(o);
        //            }
        //        }

        // GET: Track/Create
        //        [Authorize(Roles = "Coordinator")]
        //        public ActionResult Create()
        //        {
        //            var form = new TrackAddForm();
        //            form.GenreList = new SelectList(items: m.GenreGetAll(), dataValueField: "Name", dataTextField: "Name");
        //            form.AlbumList = new MultiSelectList(items: m.AlbumGetAll(), dataValueField: "Id", dataTextField: "Name");
        //            return View(form);
        //        }

        // POST: Track/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //        [Authorize(Roles = "Coordinator")]
        //        [HttpPost]
        //        public ActionResult Create(TrackAdd newItem)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(newItem);
        //            }
        //            var addedItem = m.TrackAdd(newItem);
        //            if (addedItem == null)
        //            {
        //                return View(newItem);
        //            }
        //            else
        //            {
        //                return RedirectToAction("details", new { id = addedItem.Id });
        //            }
        //        }
        //
        //        // GET: Track/Edit/5
        //        [Authorize(Roles = "Coordinator")]
        //        public ActionResult Edit(int? id)
        //        {
        //            var o = m.TrackGetByIdWithDetail(id.GetValueOrDefault());
        //            if (o == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            else
        //            {
        //                var form = m.mapper.Map<TrackWithDetail, TrackEditAlbumsForm>(o);
        //                var selectedValues = o.Albums.Select(a => a.Id);
        //                form.AlbumList = new MultiSelectList(items: m.AlbumGetAll(), dataValueField: "Id", dataTextField: "Name", selectedValues: selectedValues);
        //                return View(form);
        //            }
        //        }
        //
        //        // POST: Track/Edit/5
        //        [Authorize(Roles = "Coordinator")]
        //        [HttpPost]
        //        public ActionResult Edit(int? id, TrackEditAlbums newItem)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(newItem);
        //            }
        //
        //            if (id.GetValueOrDefault() != newItem.Id)
        //            {
        //                return RedirectToAction("index");
        //            }
        //
        //            var editedItem = m.TrackEditAlbums(newItem);
        //
        //            if (editedItem == null)
        //            {
        //                return View(newItem);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Details", new { id = newItem.Id });
        //            }
        //        }
        //
        //        // GET: Track/Delete/5
        //        [Authorize(Roles = "Coordinator")]
        //        public ActionResult Delete(int? id)
        //        {
        //            var itemToDelete = m.TrackGetByIdWithDetail(id.GetValueOrDefault());
        //
        //            if (itemToDelete == null)
        //            {
        //                return RedirectToAction("index");
        //            }
        //            else
        //            {
        //                return View(itemToDelete);
        //            }
        //        }
        //
        //        // POST: Track/Delete/5
        //        [Authorize(Roles = "Coordinator")]
        //        [HttpPost]
        //        public ActionResult Delete(int? id, FormCollection collection)
        //        {
        //            var result = m.TrackDelete(id.GetValueOrDefault());
        //            return RedirectToAction("index");
        //        }
    }
}
