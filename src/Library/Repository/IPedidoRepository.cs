using EFCore;
using Library.Enums;
using Library.ResponseDTO.Pedido;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Repository;

public interface IPedidoRepository
{
    Task<Pedido?> ObterPedido(Guid id);

    Task<Produto> ObterProduto(string id);

    Task<PedidosGetAllResponse> Listar(
        string? query,
        int quantidade,
        int? page);

    Task CriarPedido(
        Pedido pedido,
        List<PedidoItem> itens);

    Task AlterarStatus(
        Pedido pedido,
        StatusPedido status);

    Task AdicionarHistorico(
        Pedido pedido,
        string? motivo = null);

    Task<IDbContextTransaction> BeginTransaction();
}