using EFCore;
using Library.ResponseDTO.Clientes;

namespace Library.Repository
{
    public interface IClienteRepository
    {
        Task AtivarOuDesativar(Guid id, bool acao);

        bool Exists(Guid id);

        Task<Cliente?> Buscar(Guid id);

        Task<Cliente?> Buscar(int id);

        Task<ClienteListAllResponse> Listar(
            string? query,
            int quantidade,
            int? page);

        Task<Cliente> Criar(Cliente cliente);
    }
}