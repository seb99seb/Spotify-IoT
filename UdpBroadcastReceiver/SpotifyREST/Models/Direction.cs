namespace SpotifyREST.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public Direction()
        {

        }

        public Direction(int id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
