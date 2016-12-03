using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenecaMusic.Controllers
{
    public class MediaItemBase
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string StringId { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required, StringLength(200)]
        public string ContentType { get; set; }
    }

    public class MediaItemContent : MediaItemBase
    {
        public byte[] Content { get; set; }
    }

    public class MediaItemAdd
    {
        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        public HttpPostedFileBase FileUpload { get; set; }

        [Required]
        public int ArtistId { get; set; }
    }

    public class MediaItemAddForm
    {
        [Display(Name = "Descriptive caption")]
        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Media item")]
        [DataType(DataType.Upload)]
        public string FileUpload { get; set; }

        [Required]
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }
    }
}