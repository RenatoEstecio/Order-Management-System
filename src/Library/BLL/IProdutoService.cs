using EFCore;
using Library.DTO;
using Library.RequestDTO;
using Library.ResponseDTO;
using System.Threading.Tasks;

namespace Library.BLL
{
    public interface IProdutoService
    {
        Task<ProdutoListAllResponse> Listar(string query, int quantidade, int page);

        Task<Produto> Criar(ProdutoRequest request);

        Task<Produto> Alterar(ProdutoUpdateRequest request, string id);

        Task<ProdutoDetailsResponse> Buscar(string id);

        Task AtivarOuDesativar(string id, bool acao);

        Task AtualizarEstoque(string id, int quantidade);
    }
}