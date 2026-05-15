using Library.BLL;
using Library.DTO;
using Library.RequestDTO;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.AspNetCore.Mvc;


namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutoController(IProdutoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        /// <response code="200">Produto criado com sucesso.</response>
        /// <response code="409">Produto já cadastrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
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

        /// <summary>
        /// Lista produtos de forma paginada.
        /// </summary>
        /// <response code="200">Produtos listados com sucesso.</response>
        /// <response code="400">Quantidade ou página inválida.</response>
        /// <response code="404">Nenhum resultado encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
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

        /// <summary>
        /// Busca um produto pelo identificador.
        /// </summary>
        /// <response code="200">Produto encontrado com sucesso.</response>
        /// <response code="404">Produto não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoGetDetailsResponse>> GetById(string id)
        {
            try
            {
                return Ok(new ProdutoGetDetailsResponse
                {
                    Message = "Sucesso",
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

        /// <summary>
        /// Atualiza as informações de um produto.
        /// </summary>
        /// <response code="200">Produto atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Produto não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>

        [HttpPut("{id}/Dados")]
        public async Task<IActionResult> UpdateDados(string id, [FromBody] ProdutoUpdateRequest request)
        {
            try
            {
                return Ok(new ProdutoCreateResponse
                {
                    Message = "Produto Alterado com Sucesso",
                    Produto = new ProdutoDetailsResponse(await _service.Alterar(request,id))
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

        /// <summary>
        /// Ativa ou desativa um produto.
        /// </summary>
        /// <response code="200">Status do produto atualizado com sucesso.</response>
        /// <response code="404">Produto não encontrado.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
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

        /// <summary>
        /// Atualiza o estoque de um produto.
        /// </summary>
        /// <response code="200">Estoque atualizado com sucesso.</response>
        /// <response code="400">Quantidade inválida.</response>
        /// <response code="404">Produto não encontrado.</response>
        /// <response code="409">Quantidade insuficiente em estoque.</response>
        /// <response code="500">Erro Interno de Processamento.</response>
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> UpdateEstoque(string id, int quantidade)
        {
            try
            {
                await _service.AtualizarEstoque(id, quantidade);

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
