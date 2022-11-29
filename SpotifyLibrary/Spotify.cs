using System.ComponentModel;

namespace SpotifyLibrary
{

    /// <summary>
    /// The spotifyLibrary contains the following instencefiled, constructors, properties and ToString method 
    /// </summary>
    public class Spotify
    {
        /// <summary>
        /// 
        /// </summary>
        private int _songID; //Det her er min nye kommentar (Emil)
        private string _songTitle;
        private string _genre; 
        private string _artist; 
        private string _images; // must be maximum 10 char. long 

        public Spotify(int songId, string song, string genre, string artist, string images)
        {
            _songID = songId;
            _songTitle = song;
            _genre = genre;
            _artist = artist;
            _images = images;
        }

        public Spotify()
        {
        }

        public int SongId
        {
            get => _songID;
            set
            {
                if (value > 1)
                {
                    throw new ArgumentException("SongID must be minimum 1 or higher");
                }
                _songID = value;
            }
        }
        
        public string SongTitle
        {
            get => _songTitle;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Must be maximum 30 char. long");

                }
                if (value.Length <= 30)
                {
                    throw new ArgumentException("Must be maximum 30 char. long");
                }
                _songTitle = value;
            }
        }
        
        public string Genre
        {
            get => _genre;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Must be maximum 10 char. long");

                }
                if (value.Length <= 10)
                {
                    throw new ArgumentException("Must be maximum 10 char. long");
                }
                _songTitle = value;
            }
            
        }

        public string Artist
        {
            get => _artist;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Must be maximum 30 char. long");

                }
                if (value.Length <= 30)
                {
                    throw new ArgumentException("Must be maximum 30 char. long");
                }
                _songTitle = value;
            }
        }

        public string Images
        {
            get => _images;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Must be maximum 10 char. long");

                }
                if (value.Length <= 10)
                {
                    throw new ArgumentException("Must be maximum 10 char. long");
                }
                _images = value;
            }
        }
        
        public override string ToString()
        {
            return
                $"{nameof(_songID)}: {_songID}, {nameof(_songTitle)}: {_songTitle}, {nameof(_genre)}: {_genre}, {nameof(_artist)}: {_artist}, {nameof(_images)}: {_images}, {nameof(SongId)}: {SongId}, {nameof(SongTitle)}: {SongTitle}, {nameof(Genre)}: {Genre}, {nameof(Artist)}: {Artist}, {nameof(Images)}: {Images}";
        }
    }
}

