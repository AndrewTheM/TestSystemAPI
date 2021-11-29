using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Controllers
{
    [Route("api/attempts")]
    [ApiController]
    [EnableCors]
    public class AttemptController : ControllerBase
    {
        private readonly ICompletionService _service;

        public AttemptController(ICompletionService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttemptsOfUser(string id)
        {
            var attempts = await _service.GetUserAttemptsAsync(id);
            if (attempts == null)
                return NotFound();
            return Ok(attempts);
        }

        [HttpPost]
        public async Task<IActionResult> PostAttempt([FromBody] AttemptDto newAttempt)
        {
            await _service.RecordAttemptAsync(newAttempt);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttempts(string id)
        {
            await _service.ClearUserAttemptsAsync(id);
            var attempts = await _service.GetUserAttemptsAsync(id);
            return Ok(attempts);
        }
    }
}
