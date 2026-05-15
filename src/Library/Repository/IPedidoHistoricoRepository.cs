using EFCore;

namespace Library.Repository
{
    public interface IPedidoHistoricoRepository
    {
        Task<List<PedidoHistorico>> ObterHistorico(int pedidoId);
    }
}