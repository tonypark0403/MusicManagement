using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    [Authorize]
    public class ClipController : Controller
    {
        Manager m = new Manager();

        [Route("clip/{id}")]
        public ActionResult Details(int? id)
        {
            var o = m.ClipTrackGetById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(o.Clip, o.AudioType);
            }
        }
    }
}