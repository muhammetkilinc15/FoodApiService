using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTest()
        {
            Random rnd = new Random();
            return Ok($"Uguru bugün {rnd.Next(1, 100)} kez abi dedi");
        }

    }
}
