using SensifyREST.Services;
using SensifyREST.Models;

namespace SensifyREST.Managers
{
    public class MoodsManager
    {
        private static Moods _moods = new Moods();
        private static MoodsService _moodsService = new MoodsService(_moods);
        public string GetPlaylistIdByMood(string mood)
        {
            _moodsService.CheckMood(mood);
            return _moodsService.GetPlaylistId(mood);
        }
        public void SavePlaylistIdToMood(string mood, string playlistId)
        {
            _moodsService.CheckMood(mood);
            _moodsService.SavePlaylistId(mood, playlistId);
        }
    }
}
