using SpotifyLibrary;

namespace SpotifyAPI.Managers {
    public interface ISpotify
    {
        Spotify Create(string sp, Spotify sang);
    }
}
