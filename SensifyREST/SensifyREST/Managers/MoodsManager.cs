using SensifyREST.Services;

namespace SensifyREST.Managers
{
    public class MoodsManager
    {
        /// Vi bruger vores MoodService til at styre 'objekterne' i denne klasse
        private static MoodsService _moodsService = new MoodsService();

        /// Metode til at hente spillelisteID på baggrund af humøret. 
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
        public void UpdateCurrentMood(string direction)
        {
            _moodsService.UpdateCurrentMood(direction);
        }
        public string GetCurrentMood()
        {
            return _moodsService.GetCurrentMood();
        }
    }
}
