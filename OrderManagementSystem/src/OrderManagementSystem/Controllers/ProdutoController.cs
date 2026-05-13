using Library.DTO;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }
    }
}
