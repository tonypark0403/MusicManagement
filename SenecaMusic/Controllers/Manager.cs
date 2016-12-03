using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using SenecaMusic.Models;
using System.Security.Claims;

namespace SenecaMusic.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // Get AutoMapper instance
        public IMapper mapper = AutoMapperConfig.RegisterMappings();

        // Declare a property to hold the user account for the current request
        // Can use this property here in the Manager class to control logic and flow
        // Can also use this property in a controller 
        // Can also use this property in a view; for best results, 
        // near the top of the view, add this statement:
        // var userAccount = new ConditionalMenu.Controllers.UserAccount(User as System.Security.Claims.ClaimsPrincipal);
        // Then, you can use "userAccount" anywhere in the view to render content
        public UserAccount UserAccount { get; private set; }

        public Manager()
        {
            // If necessary, add constructor code here

            // Initialize the UserAccount property
            UserAccount = new UserAccount(HttpContext.Current.User as ClaimsPrincipal);

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()

        public IEnumerable<GenreBase> GenreGetAll()
        {
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBase>>(ds.Genres.OrderBy(a => a.Id));
        }

        public IEnumerable<AlbumBase> AlbumGetAll()
        {
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBase>>(ds.Albums.OrderBy(a => a.Id));
        }

        public AlbumWithDetail AlbumGetByIdWithDetail(int id)
        {
            var o = ds.Albums.Include("Tracks").Include("Artists").SingleOrDefault(a => a.Id == id);
            return (o == null) ? null : mapper.Map<Album, AlbumWithDetail>(o);
        }

        public AlbumWithDetail ArtistAlbumAdd(ArtistAlbumAdd newItem)
        {
            var o = ds.Artists.SingleOrDefault(a => a.Id == newItem.ArtistId);
            if (o == null)
            {
                return null;
            }
            else
            {
                var addedItem = ds.Albums.Add(mapper.Map<ArtistAlbumAdd, Album>(newItem));
                addedItem.Coordinator = UserAccount.Name;
                addedItem.Artists.Add(o);
                ds.SaveChanges();
                return (addedItem == null) ? null : mapper.Map<Album, AlbumWithDetail>(addedItem);
            }
        }

        public IEnumerable<ArtistBase> ArtistGetAll()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBase>>(ds.Artists.OrderBy(a => a.Id));
        }

        public ArtistWithDetail ArtistGetByIdWithDetail(int id)
        {
            var o = ds.Artists.Include("Albums").Include("MediaItems").SingleOrDefault(a => a.Id == id);
            return (o == null) ? null : mapper.Map<Artist, ArtistWithDetail>(o);
        }

        public ArtistWithDetail ArtistAdd(ArtistAdd newItem)
        {
            var addedItem = ds.Artists.Add(mapper.Map<ArtistAdd, Artist>(newItem));
            addedItem.Executive = UserAccount.Name;
            ds.SaveChanges();
            return (addedItem == null) ? null : mapper.Map<Artist, ArtistWithDetail>(addedItem);
        }

        public IEnumerable<TrackWithAlbum> TrackGetAll()
        {
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackWithAlbum>>(ds.Tracks.Include("Albums").OrderBy(t => t.Id));
        }

        public TrackBase TrackGetById(int id)
        {
            var o = ds.Tracks.SingleOrDefault(t => t.Id == id);
            return (o == null) ? null : mapper.Map<Track, TrackBase>(o);
        }

        public ClipBase ClipTrackGetById(int id)
        {
            var o = ds.Tracks.Find(id);
            return (o == null || o.AudioType == null) ? null : mapper.Map<Track, ClipBase>(o);
        }

        public TrackBase AlbumTrackAdd(AlbumTrackAdd newItem)
        {
            var o = ds.Albums.SingleOrDefault(a => a.Id == newItem.AlbumId);
            if (o == null)
            {
                return null;
            }

            var addedItem = ds.Tracks.Add(mapper.Map<AlbumTrackAdd, Track>(newItem));
            addedItem.Clerk = UserAccount.Name;
            addedItem.Albums.Add(o);

            byte[] audioBytes = new byte[newItem.AudioUpload.ContentLength];
            newItem.AudioUpload.InputStream.Read(audioBytes, 0, newItem.AudioUpload.ContentLength);
            addedItem.Clip = audioBytes;
            addedItem.AudioType = newItem.AudioUpload.ContentType;

            ds.SaveChanges();
            return (addedItem == null) ? null : mapper.Map<Track, TrackBase>(addedItem);
        }

        public ArtistWithDetail MediaItemAdd(MediaItemAdd newItem)
        {
            var artist = ds.Artists.SingleOrDefault(a => a.Id == newItem.ArtistId);
            if (artist == null)
            {
                return null;
            }

            var addedItem = ds.MediaItem.Add(mapper.Map<MediaItemAdd, MediaItem>(newItem));

            addedItem.Artist = artist;
            byte[] fileBytes = new byte[newItem.FileUpload.ContentLength];
            newItem.FileUpload.InputStream.Read(fileBytes, 0, newItem.FileUpload.ContentLength);
            addedItem.Content = fileBytes;
            addedItem.ContentType = newItem.FileUpload.ContentType;

            ds.SaveChanges();
            return (addedItem == null) ? null : ArtistGetByIdWithDetail(artist.Id);
        }

        public MediaItemContent MediaItemGetByStringId(string stringId)
        {
            var o = ds.MediaItem.SingleOrDefault(a => a.StringId == stringId);
            return (o == null) ? null : mapper.Map<MediaItem, MediaItemContent>(o);
        }

        //        public TrackWithDetail TrackGetByIdWithDetail(int id)
        //        {
        //            var o = ds.Tracks.Include("Albums.Artists").SingleOrDefault(t => t.Id == id);
        //            return (o == null) ? null : mapper.Map<Track, TrackWithDetail>(o);
        //        }
        //
        //        public TrackWithDetail TrackAdd(TrackAdd newItem)
        //        {
        //            List<Album> albums = new List<Album>();
        //            foreach (int albumid in newItem.AlbumIds)
        //            {
        //                var a = ds.Albums.Find(albumid);
        //                if (a == null)
        //                {
        //                    return null;
        //                }
        //                else
        //                {
        //                    albums.Add(a);
        //                }
        //            }
        //
        //            var addedItem = ds.Tracks.Add(mapper.Map<TrackAdd, Track>(newItem));
        //            addedItem.Clerk = UserAccount.Name;
        //            addedItem.Albums = albums;
        //            ds.SaveChanges();
        //            return (addedItem == null) ? null : mapper.Map<Track, TrackWithDetail>(addedItem);
        //        }
        //
        //        public TrackWithDetail TrackEditAlbums(TrackEditAlbums newItem)
        //        {
        //            var o = ds.Tracks.Include("Albums").SingleOrDefault(t => t.Id == newItem.Id);
        //            if (o == null)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                o.Albums.Clear();
        //                if (newItem.AlbumIds != null)
        //                {
        //                    foreach (var albumid in newItem.AlbumIds)
        //                    {
        //                        var a = ds.Albums.Find(albumid);
        //                        o.Albums.Add(a);
        //                    }
        //                }
        //                ds.SaveChanges();
        //            }
        //            return mapper.Map<Track, TrackWithDetail>(o);
        //        }
        //
        //        public bool TrackDelete(int id)
        //        {
        //            var itemToDelete = ds.Tracks.Find(id);
        //            if (itemToDelete == null)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                ds.Tracks.Remove(itemToDelete);
        //                ds.SaveChanges();
        //                return true;
        //            }
        //        }




        // Add some programmatically-generated objects to the data store
        // Can write one method, or many methods - your decision
        // The important idea is that you check for existing data first
        // Call this method from a controller action/method

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // Genre

            if (ds.Genres.Count() == 0)
            {
                // Add genres

                ds.Genres.Add(new Genre { Name = "Alternative" });
                ds.Genres.Add(new Genre { Name = "Classical" });
                ds.Genres.Add(new Genre { Name = "Country" });
                ds.Genres.Add(new Genre { Name = "Easy Listening" });
                ds.Genres.Add(new Genre { Name = "Hip-Hop/Rap" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "R&B" });
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Soundtrack" });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Artist

            if (ds.Artists.Count() == 0)
            {
                // Add artists

                ds.Artists.Add(new Artist
                {
                    Name = "The Beatles",
                    BirthOrStartDate = new DateTime(1962, 8, 15),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/9/9f/Beatles_ad_1965_just_the_beatles_crop.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Adele",
                    BirthName = "Adele Adkins",
                    BirthOrStartDate = new DateTime(1988, 5, 5),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "http://www.billboard.com/files/styles/article_main_image/public/media/Adele-2015-close-up-XL_Columbia-billboard-650.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Bryan Adams",
                    BirthOrStartDate = new DateTime(1959, 11, 5),
                    Executive = user,
                    Genre = "Rock",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Bryan_Adams_Hamburg_MG_0631_flickr.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Album

            if (ds.Albums.Count() == 0)
            {
                // Add albums

                // For Bryan Adams
                var bryan = ds.Artists.SingleOrDefault(a => a.Name == "Bryan Adams");

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "Reckless",
                    ReleaseDate = new DateTime(1984, 11, 5),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/5/56/Bryan_Adams_-_Reckless.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "So Far So Good",
                    ReleaseDate = new DateTime(1993, 11, 2),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/pt/a/ab/So_Far_so_Good_capa.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Track

            if (ds.Tracks.Count() == 0)
            {
                // Add tracks

                // For Reckless
                var reck = ds.Albums.SingleOrDefault(a => a.Name == "Reckless");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Run To You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Heaven",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Somebody",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Summer of '69",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Kids Wanna Rock",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                // For Reckless
                var so = ds.Albums.SingleOrDefault(a => a.Name == "So Far So Good");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Straight from the Heart",
                    Composers = "Bryan Adams, Eric Kagna",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "It's Only Love",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "This Time",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "(Everything I Do) I Do It for You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Heat of the Night",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.SaveChanges();
                done = true;
            }

            // Load role claims
            if (ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Manager" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Programmer" });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    // New "UserAccount" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it
    public class UserAccount
    {
        // Constructor, pass in the security principal
        public UserAccount(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        // Add other role-checking properties here as needed
        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }
}