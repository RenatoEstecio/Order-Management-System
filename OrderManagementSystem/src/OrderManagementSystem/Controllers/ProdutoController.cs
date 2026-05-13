using Library.BLL;
using Library.DTO;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.AspNetCore.Mvc;


namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoCreateResponse>> Create(ProdutoRequest produtoRequest)
        {
            try
            {
                return Ok(new ProdutoCreateResponse
                {
                    Message = "Produto criado com Sucesso",
                    Produto = new ProdutoDetailsResponse(await _service.Criar(produtoRequest))
                });
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ProdutoCreateResponse
                {
                    Message = ex.Message,
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProdutoCreateResponse());
            }

        }

        [HttpGet]
        public async Task<ActionResult<ProdutoListAllResponse>> GetAll(string? query, int quantidade = 10, int page = 1)
        {
            try
            {
                return Ok(await _service.Listar(query, quantidade, page));
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ProdutoListAllResponse
                {
                    Message = ex.Message,
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProdutoCreateResponse());
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoGetDetailsResponse>> GetById(string id)
        {
            try
            {
                return Ok(new ProdutoGetDetailsResponse
                {
                    Message = "Produto criado com Sucesso",
                    Produto = await _service.Buscar(id)
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

        [HttpPut("{id}/Dados")]
        public async Task<IActionResult> UpdateDados(string id, [FromBody] ProdutoRequest request)
        {
            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ResponseBase>> UpdateStatus(string id, bool Ativo)
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

        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> UpdateEstoque(string id, int adicionar)
        {
            return Ok();
        }
    }
}
