using Library.DTO;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(ClienteRequest cliente)
        {
            return Ok();
        }
    }
}
