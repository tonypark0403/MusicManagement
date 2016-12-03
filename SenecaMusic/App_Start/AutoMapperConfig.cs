using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;

namespace SenecaMusic
{
    public static class AutoMapperConfig
    {
        public static IMapper RegisterMappings()
        {
            // Create map statements - using AutoMapper instance API
            // new MapperConfiguration(cfg => cfg.CreateMap< FROM , TO >());
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

                // Add more below...
                cfg.CreateMap<Models.Genre, Controllers.GenreBase>();

                cfg.CreateMap<Models.Album, Controllers.AlbumBase>();
                cfg.CreateMap<Models.Album, Controllers.AlbumWithDetail>();
                cfg.CreateMap<Controllers.ArtistAlbumAdd, Models.Album>();

                cfg.CreateMap<Models.Artist, Controllers.ArtistBase>();
                cfg.CreateMap<Models.Artist, Controllers.ArtistWithDetail>();
                cfg.CreateMap<Controllers.ArtistAdd, Models.Artist>();

                cfg.CreateMap<Models.Track, Controllers.TrackBase>();
                cfg.CreateMap<Models.Track, Controllers.TrackWithAlbum>();
                cfg.CreateMap<Models.Track, Controllers.ClipBase>();
                cfg.CreateMap<Controllers.AlbumTrackAdd, Models.Track>();

                cfg.CreateMap<Models.MediaItem, Controllers.MediaItemBase>();
                cfg.CreateMap<Models.MediaItem, Controllers.MediaItemContent>();
                cfg.CreateMap<Controllers.MediaItemAdd, Models.MediaItem>();

            });

            var mapper = config.CreateMapper();
            // or: IMapper mapper = new Mapper(config);
            return mapper;

        }
    }
}