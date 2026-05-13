using Library.DTO;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {             
        [HttpPost]
        public async Task<IActionResult> Create(ListaPedidoRequest listaPedido)
        {          
            return Ok();
        }
    }
}
