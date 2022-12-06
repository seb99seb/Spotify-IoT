using SensifyREST.models;
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
        /*
        get mood
        forbind mood to playlist
        hint playlist kommer an på mood
        */
        public int GetPlaylistId ()
        {
            //SQL here
            string sql = $"SELECT id FROM Mood WHERE id=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                return id;
            }
        }        
    }
}
