using Microsoft.AspNetCore.Mvc;
using SpotifyREST.Managers;
using SpotifyREST.Models;

namespace SpotifyREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectionController : ControllerBase
    {
        [HttpPost]
        public IActionResult Get(string message)
        {
            DirectionManager.Create(message);

            return StatusCode(201);
        }

        [HttpGet]
        [Route("id")]
        public IActionResult Get(int id)
        {
            return Ok(DirectionManager.Get(id));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Direction> directions = DirectionManager.GetAll();
            
            if (directions.Count > 0)
            {
                return Ok(directions);
            }
            else
            {
                return NoContent();
            }
        }
    }
}