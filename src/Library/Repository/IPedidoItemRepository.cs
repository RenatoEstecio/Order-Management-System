using Library.ResponseDTO;
using EFCore;
namespace Library.Repository
{
    public interface IPedidoItemRepository
    {
        Task<List<PedidoItemList>> ObterProdutos(int pedidoId);
    }
}