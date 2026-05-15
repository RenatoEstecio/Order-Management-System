using Library.BLL;
using Library.DTO;
using Library.Enums;
using Library.RequestDTO;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.ResponseDTO.Clientess;
using Library.UTIL;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza a criação de um pedido.
        /// </summary>
        /// <response code="200">Pedido criado com sucesso.</response>
        /// <response code="400">Dados do pedido inválidos.</response>
        /// <response code="401">Cliente não autorizado.</response>
        /// <response code="404">Cliente ou produto não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpPost]
        public async Task<IActionResult> Create(ListaPedidoRequest listaPedido)
        {
            try
            {
                await _service.ExecutarPedido(listaPedido);

                return Ok(new ResponseBase
                {
                    Message = "Pedido criado com Sucesso",                 
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

        /// <summary>
        /// Lista pedidos de forma paginada.
        /// </summary>
        /// <response code="200">Pedidos listados com sucesso.</response>
        /// <response code="400">Quantidade ou página inválida.</response>
        /// <response code="404">Nenhum resultado encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpGet]
        public async Task<ActionResult<ClienteListAllResponse>> GetAll(string? query, int quantidade = 10, int page = 1)
        {
            try
            {
                return Ok(await _service.Listar(query, quantidade, page));
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ClienteListAllResponse
                {
                    Message = ex.Message,
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ClienteCreateResponse());
            }
        }

        /// <summary>
        /// Busca um pedido pelo identificador.
        /// </summary>
        /// <response code="200">Pedido encontrado com sucesso.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                PedidoGetDetailsResponse pedido = await _service.Buscar(id);
                pedido.Message = "Sucesso";

                return Ok(pedido);
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

        /// <summary>
        /// Altera o status de um pedido.
        /// </summary>
        /// <response code="200">Status do pedido alterado com sucesso.</response>
        /// <response code="401">Transição de status inválida.</response>
        /// <response code="404">Pedido não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] PedidoAlteracaoRequest pedidoAlteracaoRequest)
        {
            try
            {
                await _service.AlterarStatus(
                    id, 
                    (StatusPedido)pedidoAlteracaoRequest.alteracao, 
                    pedidoAlteracaoRequest.Motivo);

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
