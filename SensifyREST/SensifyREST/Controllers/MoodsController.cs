using SensifyREST.Managers;
using Microsoft.AspNetCore.Mvc;

namespace SensifyREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodsController
    {
        private MoodsManager MM = new MoodsManager();
        [HttpGet]
        [Route("playlistId/{mood}")]
        public string Get(string mood)
        {
            return MM.GetPlaylistIdByMood(mood);
        }
        [HttpPut]
        [Route("playlistId")]
        public void Put(string mood, string playlistId)
        {
            MM.SavePlaylistIdToMood(mood, playlistId);
        }
    }
}
