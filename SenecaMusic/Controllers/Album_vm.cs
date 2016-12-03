using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    public class AlbumAdd
    {
        public AlbumAdd()
        {
            ReleaseDate = DateTime.Now.AddYears(-40);
        }
        [Display(Name = "Album name")]
        public string Name { get; set; }
        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }
        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "URL to album image (cover art)")]
        public string UrlAlbum { get; set; }
        [Display(Name = "Album description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class ArtistAlbumAdd : AlbumAdd
    {
        public int ArtistId { get; set; }
    }

    public class ArtistAlbumAddForm : ArtistAlbumAdd
    {
        public string ArtistName { get; set; }
        public SelectList GenreList { get; set; }
    }

    public class AlbumBase : AlbumAdd
    {
        public AlbumBase()
        {
            ReleaseDate = DateTime.Now.AddYears(-40);
        }
        public int Id { get; set; }
    }

    public class AlbumWithDetail : AlbumBase
    {
        public AlbumWithDetail()
        {
            Artists = new List<ArtistBase>();
            Tracks = new List<TrackBase>();
        }
        public string Coordinator { get; set; }

        //VM -> IEnumerable, Model -> ICollection
        public IEnumerable<ArtistBase> Artists { get; set; }
        public IEnumerable<TrackBase> Tracks { get; set; }
    }
}