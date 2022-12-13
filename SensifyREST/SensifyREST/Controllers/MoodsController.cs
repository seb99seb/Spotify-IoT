using SensifyREST.Managers;
using Microsoft.AspNetCore.Mvc;

namespace SensifyREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodsController
    {
        private MoodsManager MM = new MoodsManager();

        /// Håndterer HTTPget
        /// henter spilleliste ud fra humøret som string
        [HttpGet]
        [Route("playlistId/{mood}")]
        public string Get(string mood)
        {
            return MM.GetPlaylistIdByMood(mood);
        }

        /// HTTP put
        /// returnerer ikke noget, men gemmer spillelisten ved hjælp fra manageren 
        [HttpPut]
        [Route("playlistId")]
        public void Put(string mood, string playlistId)
        {
            MM.SavePlaylistIdToMood(mood, playlistId);
        }

        /// Henter og returnerer CurrentMood
        [HttpGet]
        public string GetCurrentMood()
        {
            return MM.GetCurrentMood();
        }

        /// Opdaterer det nuværende humør,
        /// tager retningen og bruger den til at opdatere humøret
        [HttpPut]
        public void PutCurrentMood(string direction)
        {
            MM.UpdateCurrentMood(direction);
        }
    }
}
