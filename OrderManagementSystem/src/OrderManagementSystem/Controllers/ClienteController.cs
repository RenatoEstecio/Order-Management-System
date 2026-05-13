using Library.BLL;
using Library.DTO;
using Library.Repository;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.ResponseDTO.Clientess;
using Library.UTIL;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }
       
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteGetDetailsResponse>> GetById(Guid id)
        {
            try
            {
                return Ok(new ClienteGetDetailsResponse
                {
                    Message = "Cliente criado com Sucesso",
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
