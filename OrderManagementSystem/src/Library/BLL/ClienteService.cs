using EFCore;
using Library.DTO;
using Library.Repository;
using Library.ResponseDTO.Clientes;
using Library.ResponseDTO.Clientess;
using Library.UTIL;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.BLL
{
    public class ClienteService
    {
        private readonly ClienteRepository _repository;

        public ClienteService(ClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Cliente> Criar(ClienteRequest request)
        {
            Cliente cliente = new Cliente();
            cliente.Nome = request.Nome.Trim();
            cliente.Email = request.Email.Trim();
            cliente.Documento = Regex.Replace(request.Documento, @"\D", "");
            cliente.CreatedAt = cliente.UpdatedAt = DateTimeHelper.ToSaoPaulo();
            cliente.Ativo = true;

            Validar(cliente);
            
            return await _repository.Criar(cliente);
        }

        public async Task<ClienteListAllResponse> Listar(string query, int quantidade, int page)
        {
            if (quantidade > 20 || quantidade < 1)
                throw new CustomException("Quantidade inválida", HttpStatusCode.BadRequest);

            if (page < 1)
                throw new CustomException("Página inválida", HttpStatusCode.BadRequest);         

            var result = await _repository.Listar(query, quantidade, page);

            if(result.Clientes.Count == 0)
                throw new CustomException("Nenhum Resultado Encontrado", HttpStatusCode.NotFound);

            return result;
        }
        public async Task AtivarOuDesativar(Guid id, bool acao)
        {
            if (!_repository.Exists(id))
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);
         
            await _repository.AtivarOuDesativar(id, acao);
        }

        public async Task<ClienteDetailsResponse> Buscar(Guid id)
        {            
            Cliente? cliente = await _repository.Buscar(id);

            if (cliente is null)
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);

            return new ClienteDetailsResponse(cliente);
        }

        public void Validar(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                throw new CustomException("Nome é obrigatório", HttpStatusCode.BadRequest);

            if (!EmailValido(cliente.Email))
                throw new CustomException("E-mail inválido", HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(cliente.Documento))
                throw new CustomException("Documento obrigatório", HttpStatusCode.BadRequest);

            if (!DocumentoValido(cliente.Documento))
                throw new CustomException("CPF/CNPJ inválido", HttpStatusCode.BadRequest);
        }

        private static bool EmailValido(string email)
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private static bool DocumentoValido(string documento)
        {
            return DocumentoValidator.IsValid(documento);
        }
    }
}
