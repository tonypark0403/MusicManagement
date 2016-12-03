using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Win32;

namespace SenecaMusic.Controllers
{
    [Authorize]
    public class MediaItemController : Controller
    {
        Manager m = new Manager();

        [Route("media/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            var o = m.MediaItemGetByStringId(stringId);
            return (o == null) ? null : File(o.Content, o.ContentType);
        }

        [Route("media/{stringId}/download")]
        public ActionResult DetailsDownload(string stringId = "")
        {
            var o = m.MediaItemGetByStringId(stringId);

            if (o == null)
            {
                return HttpNotFound();
            }
            string extension;
            RegistryKey key;
            object value;

            // Open the Registry, attempt to locate the key
            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + o.ContentType, false);
            // Attempt to read the value of the key
            value = (key == null) ? null : key.GetValue("Extension", null);
            // Build/create the file extension string
            extension = (value == null) ? string.Empty : value.ToString();

            // Create a new Content-Disposition header
            var cd = new System.Net.Mime.ContentDisposition
            {
                // Assemble the file name + extension
                FileName = $"img-{stringId}{extension}",
                // Force the media item to be saved (not viewed)
                Inline = false
            };
            // Add the header to the response
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(o.Content, o.ContentType);
        }
    }
}