using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Controllers
{
    [Route("api/tests")]
    [ApiController]
    [EnableCors]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _service.GetAllTests();
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest(int id)
        {
            var test = await _service.GetTestById(id);
            if (test == null)
                return NotFound();
            return Ok(test);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] TestDto test)
        {
            await _service.CreateTest(test);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTest([FromBody] TestDto test)
        {
            await _service.UpdateTest(test);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            bool success = await _service.RemoveTest(id);
            if (!success)
                return NotFound();
            return Ok();
        }
    }
}
