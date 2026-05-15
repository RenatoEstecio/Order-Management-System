using Library.BLL;
using Library.DTO;
using Library.Repository;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;

using Library.UTIL;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <response code="200">Cliente criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Cliente já cadastrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>

        [HttpPost]            
        public async Task<ActionResult<ClienteCreateResponse>> Create(ClienteRequest clienteRequest)       
        {         
            try
            {                
                return Ok(new ClienteCreateResponse
                {
                    Message = "Cliente criado com Sucesso",
                    Cliente = new ClienteDetailsResponse(await _service.Criar(clienteRequest))
                });
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ClienteCreateResponse
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
        /// Lista clientes de forma paginada.
        /// </summary>
        /// <response code="200">Clientes listados com sucesso.</response>
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
        /// Busca um cliente pelo identificador.
        /// </summary>
        /// <response code="200">Cliente encontrado com sucesso.</response>
        /// <response code="404">Cliente não encontrado ou identificador inválido.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteGetDetailsResponse>> GetById(Guid id)
        {
            try
            {
                return Ok(new ClienteGetDetailsResponse
                {
                    Message = "Sucesso",
                    Cliente = await _service.Buscar(id)
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
        /// Ativa ou desativa um cliente.
        /// </summary>
        /// <response code="200">Status do cliente atualizado com sucesso.</response>
        /// <response code="404">Cliente não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ResponseBase>> UpdateStatus(Guid id, bool Ativo)
        {
            try
            {
                await _service.AtivarOuDesativar(id, Ativo);

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
