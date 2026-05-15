using EFCore;
using Library.DTO;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.Repository
{
    public class ClienteRepository 
    {       
        private readonly ContextEFCore _context;

        public ClienteRepository(ContextEFCore context)
        {
            _context = context;
        }   

        public async Task AtivarOuDesativar(Guid id, bool acao)
        {
            await _context.Cliente
           .Where(x => x.Id == id)
           .ExecuteUpdateAsync(setters =>
               setters.SetProperty(x => x.Ativo, acao));
        }

        public bool Exists(Guid id)
        {
            return _context.Cliente.Any(x => x.Id == id);
        }

        public Task<Cliente?> Buscar(Guid id)
        {
            return _context.Cliente.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public Task<Cliente?> Buscar(int id)
        {
            return _context.Cliente.Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        }
        public async Task<ClienteListAllResponse> Listar(string? query, int quantidade, int? page)
        {
            var consulta = _context.Cliente.AsQueryable();

            ClienteListAllResponse response = new ClienteListAllResponse();

            if (!string.IsNullOrWhiteSpace(query))
            {
                consulta = consulta.Where(x =>
                    x.Nome.Contains(query) ||
                    x.Email.Contains(query) ||
                    x.Documento.Contains(query));
            }

            var totalItems = await consulta.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / quantidade);

            var skip = ((page ?? 1) - 1) * quantidade;

            var result = await consulta
                .OrderBy(x => x.Nome)
                .Skip(skip)
                .Take(quantidade)
                .Select(x => new ClienteListResponse
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Email = x.Email                   
                })
                .ToListAsync();

            response.Resultados = totalItems;
            response.Pagina = skip + 1;
            response.TotalPagina = totalPages;
            response.Clientes = result;
            response.Message = "Sucesso";

            return response;
        }

        public async Task<Cliente> Criar(Cliente cliente)
        {                   
            try
            {
                await _context.Cliente.AddAsync(cliente);
                await _context.SaveChangesAsync();

                return cliente;
            }            
            catch (Exception ex)
            {
                if(ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    throw new CustomException("Cliente já cadastrado", HttpStatusCode.Conflict);
                else
                    throw new Exception("Erro ao cadastrar cliente");
            }          
        }     
    }
}
