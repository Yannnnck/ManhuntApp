// Datei: src/Manhunt.Backend/Controllers/DebugController.cs
using Microsoft.AspNetCore.Mvc;

namespace Manhunt.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        [HttpGet("raw-token")]
        public IActionResult GetRawToken()
        {
            // Gibt den rohen Authorization-Header zurück
            var raw = Request.Headers["Authorization"].ToString();
            return Ok(new { RawAuthorizationHeader = raw });
        }
    }
}
