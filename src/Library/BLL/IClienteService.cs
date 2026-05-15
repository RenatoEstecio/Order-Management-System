using EFCore;
using Library.DTO;
using Library.ResponseDTO.Clientes;

namespace Library.BLL
{
    public interface IClienteService
    {
        Task<Cliente> Criar(ClienteRequest clienteRequest);

        Task<ClienteListAllResponse> Listar(string query, int quantidade, int page);

        Task<ClienteDetailsResponse> Buscar(Guid id);

        Task AtivarOuDesativar(Guid id, bool ativo);
    }
}