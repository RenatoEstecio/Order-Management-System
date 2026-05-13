using Library.BLL;
using Library.DTO;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        [HttpPost]
        public async Task<IActionResult> Create(ListaPedidoRequest listaPedido)
        {          
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int quantidade, int? page = 1)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, bool Ativo)
        {
            try
            {
               // await _service.AtivarOuDesativar(id, Ativo);

                return Ok(new ResponseBase
                {
                    Message = "Alterado",
                });
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ResponseBase
                {
                    Message = ex.Message,
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseBase());
            }
        }
    }
}
