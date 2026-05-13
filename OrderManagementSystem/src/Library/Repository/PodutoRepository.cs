using EFCore;
using Library.DTO;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.Repository
{
    public class ProdutoRepository
    {
        private readonly ContextEFCore _context;

        public ProdutoRepository(ContextEFCore context)
        {
            _context = context;
        }

        public async Task AtivarOuDesativar(string id, bool acao)
        {
            await _context.Produto
           .Where(x => x.Id == id)
           .ExecuteUpdateAsync(setters =>
               setters.SetProperty(x => x.Ativo, acao));
        }

        public bool Exists(string id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }

        public Task<Produto?> Buscar(string id)
        {
            return _context.Produto.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<ProdutoListAllResponse> Listar(string? query, int quantidade, int? page)
        {
            var consulta = _context.Produto.AsQueryable();

            ProdutoListAllResponse response = new ProdutoListAllResponse();

            if (!string.IsNullOrWhiteSpace(query))
            {
                consulta = consulta.Where(x =>
                    x.Nome.Contains(query) ||
                    x.Descricao.Contains(query) ||
                    x.Id.Contains(query));
            }

            var totalItems = await consulta.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / quantidade);

            var skip = ((page ?? 1) - 1) * quantidade;

            var result = await consulta
                .OrderBy(x => x.Nome)
                .Skip(skip)
                .Take(quantidade)
                .Select(x => new ProdutoListResponse
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Preco = x.EstoqueDisponivel
                })
                .ToListAsync();

            response.Resultados = totalItems;
            response.Pagina = skip + 1;
            response.TotalPagina = totalPages;
            response.Produtos = result;
            response.Message = "Sucesso";

            return response;
        }

        public async Task<Produto> Criar(Produto produto)
        {
            try
            {
                await _context.Produto.AddAsync(produto);
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unique"))
                    throw new CustomException("Erro: Produto já cadastrado", HttpStatusCode.BadRequest);
                else
                    throw new Exception($"Erro ao cadastrar produto: {ex.Message}");
            }
            finally
            {
                await _context.DisposeAsync();
            }
        }
    }
}
