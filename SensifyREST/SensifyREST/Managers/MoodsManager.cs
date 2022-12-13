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

        /// void - en metode der ikke returnerer noget
        /// Her bruger vi den til at gemme spillelisteID'et sammen med et humør
        public void SavePlaylistIdToMood(string mood, string playlistId)
        {
            _moodsService.CheckMood(mood);
            _moodsService.SavePlaylistId(mood, playlistId);
        }

        /// Denne metode opdaterer humøret, på baggrund af retningen af joystikket. 
        public void UpdateCurrentMood(string direction)
        {
            _moodsService.UpdateCurrentMood(direction);
        }

        /// Henter det sidst registrerede humør, og sætter det til det nuværende humør. 
        public string GetCurrentMood()
        {
            return _moodsService.GetCurrentMood();
        }
    }
}
