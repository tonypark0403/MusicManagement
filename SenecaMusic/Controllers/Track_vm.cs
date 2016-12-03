using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    public class TrackBase
    {

        public int Id { get; set; }
        [Display(Name = "Track name")]
        public string Name { get; set; }
        [Display(Name = "Composer Names (comma-separated)")]
        public string composers { get; set; }
        public string Genre { get; set; }
    }

    public class TrackWithAlbum : TrackBase
    {
        public TrackWithAlbum()
        {
            Albums = new List<AlbumBase> { };
        }
        public IEnumerable<AlbumBase> Albums { get; set; }
    }

    public class AlbumTrackAdd
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Composers { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int AlbumId { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }
    }

    public class AlbumTrackAddForm
    {
        [Display(Name = "Track name")]
        [Required, StringLength(200)]
        public string Name { get; set; }

        [Display(Name = "Composer Names (comma-separated)")]
        [Required, StringLength(500)]
        public string Composers { get; set; }

        [Display(Name = "Track genre")]
        [Required]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Sample clip")]
        [DataType(DataType.Upload)]
        public string AudioUpload { get; set; }
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public SelectList GenreList { get; set; }
    }

    public class ClipBase
    {
        public string AudioType { get; set; }

        public byte[] Clip { get; set; }
    }
}