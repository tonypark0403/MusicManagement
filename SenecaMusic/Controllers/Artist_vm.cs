using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenecaMusic.Controllers
{
    public class ArtistAdd
    {

        public ArtistAdd()
        {
            BirthOrStartDate = DateTime.Now.AddYears(-40);
        }
        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }
        [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }
        [Display(Name = "Birth date, or start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }
        [Display(Name = "URL to artist photo")]
        public string UrlArtist { get; set; }
        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }
        [Display(Name = "Artist profile")]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }
    }
    public class ArtistAddForm : ArtistAdd
    {
        public SelectList GenreList { get; set; }
    }

    public class ArtistBase : ArtistAdd
    {
        public int Id { get; set; }
    }

    public class ArtistWithDetail : ArtistBase
    {
        public ArtistWithDetail()
        {
            Albums = new List<AlbumBase>();
            MediaItems = new List<MediaItemBase>();
        }
        public string Executive { get; set; }
        public int AlbumsCount { get; set; }

        //VM -> IEnumerable, Model -> ICollection
        public IEnumerable<AlbumBase> Albums { get; set; }
        public IEnumerable<MediaItemBase> MediaItems { get; set; }
    }
}