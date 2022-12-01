using SpotifyREST.Models;

namespace SpotifyREST.Managers
{
    public class DirectionManager
    {
        private static int Count;

        private static List<Direction> Directions = new List<Direction>();

        public static void Create(string message)
        {
            Count += 1;

            Directions.Add(new Direction(Count, message));
        }

        public static Direction Get(int id)
        {
            if (!Directions.Exists(x => x.Id == id))
            {
                throw new KeyNotFoundException();
            }

            return Directions.Find(x => x.Id == id);
        }

        public static List<Direction> GetAll()
        {
            return Directions;
        }
    }
}