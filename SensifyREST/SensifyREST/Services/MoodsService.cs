using SensifyREST.Models;
using System.Data.SqlClient;

namespace SensifyREST.Services
{
    public class MoodsService
    {
        public Moods Moods { get; set; } 

        private const string connectionString = @"Server=tcp:eventzealand.database.windows.net,1433;Initial Catalog=Spotify;Persist Security Info=False;User ID=sovs;Password=password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public MoodsService(Moods moods)
        {
            Moods = moods;
        }
        public string CheckMood(string mood)
        {
            switch (mood)
                {
                case "happy":
                    return mood;

                case "sad":
                    return mood;

                case "neutral":
                    return mood;

                default:
                    throw new ArgumentException("find et rigtigt humør!!");
            }
        }
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
    }
}
