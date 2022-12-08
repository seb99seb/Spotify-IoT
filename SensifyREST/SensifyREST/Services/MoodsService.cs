using System.Data.SqlClient;

namespace SensifyREST.Services
{
    public class MoodsService
    {
        /// <summary>
        /// Connection string used for connecting to our database
        /// </summary>
        private const string connectionString = @"Server=tcp:eventzealand.database.windows.net,1433;Initial Catalog=Spotify;Persist Security Info=False;User ID=sovs;Password=password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public MoodsService()
        {
            
        }
        /// <summary>
        /// Method that makes sure that the given mood parameter is valid for our database
        /// </summary>
        /// <param name="mood">A string that get checked by the method</param>
        /// <returns>Returns either "Happy", "Sad" or "Neutral"</returns>
        /// <exception cref="ArgumentException">Throws exception if its not a valid mood string</exception>
        public string CheckMood(string mood)
        {
            switch (mood)
                {
                case "Happy":
                    return mood;

                case "Sad":
                    return mood;

                case "Neutral":
                    return mood;

                default:
                    throw new ArgumentException("find et rigtigt humør!!");
            }
        }
        /// <summary>
        /// Method that creates a connection to our database in order to get a playlist id via a mood string
        /// </summary>
        /// <param name="mood">A string that is used to get the coorsponding playlist id</param>
        /// <returns>Returns the id of a Spotify playlist</returns>
        public string GetPlaylistId (string mood)
        {
            CheckMood(mood);
            //SQL here
            string sql = $"SELECT {mood} FROM Mood WHERE {mood}={mood}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                string id = cmd.ExecuteScalar().ToString().Trim();
                return id;
            }
        }
        /// <summary>
        /// Saves the id of a Spotify playlist to the database, connected to the given "mood" parameter
        /// </summary>
        /// <param name="mood">A string that is used to get the coorsponding playlist id</param>
        /// <param name="playlistId">The id of a Spotify playlist</param>
        public void SavePlaylistId(string mood, string playlistId)
        {
            CheckMood(mood);
            string sql = $"UPDATE Mood SET {mood} = '{playlistId}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.ExecuteReader();
            }
        }

        public void UpdateCurrentMood(string direction)
        {
            string sql = $"UPDATE Mood SET CurrentMood = '{CheckDirection(direction)}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.ExecuteReader();
            }
        }

        private string CheckDirection(string direction)
        {
            switch (direction)
            {
                case "up":
                    return "Neutral";
                case "down":
                    return "Stop";
                case "left":
                    return "Happy";
                case "right":
                    return "Sad";
                case "middle":
                    return "Stop";
                default:
                    throw new ArgumentException();
            }
        }
        public string GetCurrentMood()
        {
            string sql = $"SELECT CurrentMood FROM Mood";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                string CurrentMood = cmd.ExecuteScalar().ToString().Trim();
                return CurrentMood;
            }
        }
    }
}
