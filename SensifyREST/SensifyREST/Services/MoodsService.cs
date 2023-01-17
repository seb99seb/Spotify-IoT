using System.Data.SqlClient;

namespace SensifyREST.Services
{
    public class MoodsService
    {
        /// <summary>
        /// Connection string used for connecting to our database
        /// </summary>
        //old connection string
        //private const string connectionString = @"Server=tcp:eventzealand.database.windows.net,1433;Initial Catalog=Spotify;Persist Security Info=False;User ID=sovs;Password=password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //new working one
        private const string connectionString = @"Server=tcp:rb.database.windows.net,1433;Initial Catalog=RBDB;Persist Security Info=False;User ID=RBAdmin;Password=Hemmeligt88!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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
        /// <summary>
        /// Void method that uses SQL to update the "CurrentMood" value in the database using the "direction" string given as a parameter
        /// </summary>
        /// <param name="direction">The string given to the method, used for determining which mood to put into the database</param>
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
        /// <summary>
        /// Checks if the given string is either "up", "down", "right", "left" or "middle" - depending on
        /// which direction is given, it will return a different string, corrosponding to a mood or "Stop",
        /// a signal send on the tell that there is now no mood
        /// </summary>
        /// <param name="direction">The string given to the method, used for determining which string to return</param>
        /// <returns>Will return either "Neutral", "Stop", "Happy", "Sad", "Stop" or a ArgumentExeption</returns>
        /// <exception cref="ArgumentException">If the parameter "direction" is not a valid direction, it will return this exception</exception>
        private string CheckDirection(string direction)
        {
            //switch statement that looks at all the valid values for direction to have
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

        /// <summary>
        /// Method that open a SQL connection to then get the mood that is currently saved in the database
        /// </summary>
        /// <returns>Returns the mood saved in the database as a string</returns>
        public string GetCurrentMood()
        {
            string sql = $"SELECT CurrentMood FROM Mood";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                //"ExecuteScalar()" to only get one value from the database, "ToString()" to make it a string to easily return it,
                //and "Trim()" to make sure theres no blank spaces behind the mood
                string CurrentMood = cmd.ExecuteScalar().ToString().Trim();
                return CurrentMood;
            }
        }
    }
}
