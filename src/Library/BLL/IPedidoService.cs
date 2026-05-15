using EFCore;
using Library.DTO;
using Library.Enums;
using Library.ResponseDTO;
using Library.ResponseDTO.Pedido;

namespace Library.BLL
{
    public interface IPedidoService
    {
        Task<PedidoGetDetailsResponse> Buscar(Guid id);

        Task ExecutarPedido(ListaPedidoRequest pedidoLista);

        Task<PedidosGetAllResponse> Listar(string query, int quantidade, int page);

        Task AlterarStatus(Guid pedido, StatusPedido novoStatus, string? motivo = null);

        Task AlterarStatus(Pedido pedido, StatusPedido novoStatus, string? motivo = null);
    }
}