using EFCore;
using Library.ResponseDTO;

namespace Library.Repository
{
    public interface IProdutoRepository
    {
        Task AtivarOuDesativar(string id, bool acao);

        Task<bool> AtualizarEstoque(string id, int quantidade);

        bool Exists(string id);

        Task<Produto?> Buscar(string id);

        Task<ProdutoListAllResponse> Listar(
            string? query,
            int quantidade,
            int? page);

        Task<Produto> Criar(Produto produto);

        Task<Produto> Alterar(Produto produto);
    }
}