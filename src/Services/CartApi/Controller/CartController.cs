using System.Net;
using System.Threading.Tasks;
using CartApi.Domain;
using CartApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CartApi.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _repo;

        public CartController(ILogger<CartController> logger, ICartRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var basket = await _repo.GetAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Cart value)
        {
            var basket = await _repo.UpdateAsync(value);
            return Ok(basket);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _repo.DeleteAsync(id);
        }

    }
}