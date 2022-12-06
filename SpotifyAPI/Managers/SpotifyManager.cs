using SpotifyAPI.Managers;
using SpotifyLibrary;

namespace SpotifyREST.Managers {
    public class SpotifyManager : ISpotify
    {

        private static List<Spotify> _spotifySongs = new List<Spotify>()
        {
            new Spotify(1, "Roar", "Pop", "Katy Perry", "image"),
        };

        public Spotify Create(string sp, Spotify sang)
        {
            throw new NotImplementedException();
        }
    }
}
