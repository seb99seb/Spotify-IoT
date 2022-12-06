namespace SensifyREST.models
{
    public class Moods
    {
        /// <summary>
        /// Properties
        /// </summary>
        private string _mood;

        public string PlaylistID { get; set; }

        public DateTime CurrentDate { get; set; }

        public Moods()
        {

        }
        public string Mood
        {
            get => _mood;
            set
            {
                switch (value)
                {
                    case "happy":
                        _mood=value;
                        break;

                    case "sad":
                        _mood = value;
                        break;

                    case "neutral":
                        _mood = value;
                        break;

                    default:
                        throw new ArgumentException("find et rigtigt humør!!");
                }
            }
        }
    }
}
