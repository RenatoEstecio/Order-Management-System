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

        public async Task<bool> AtualizarEstoque(string id, int quantidade)
        {
            var linhasAfetadas = await _context.Produto
            .Where(x =>
                x.Id == id &&
                (x.EstoqueDisponivel + quantidade) >= 0)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    x => x.EstoqueDisponivel,
                    x => x.EstoqueDisponivel + quantidade
            ));

            return linhasAfetadas > 0;
        }

        public bool Exists(string id)
        {
            return _context.Produto.Any(x => x.Id == id);
        }

        public Task<Produto?> Buscar(string id)
        {
            return _context.Produto.Where(x => x.Id == id).FirstOrDefaultAsync();
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
                    Preco = x.Preco,
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
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                    throw new CustomException("Produto já cadastrado", HttpStatusCode.Conflict);
                else
                    throw new Exception("Erro ao cadastrar produto");
            }            
        }

        public async Task<Produto> Alterar(Produto produto)
        {
            try
            {
                _context.Produto.Attach(produto);

                _context.Entry(produto).Property(x => x.Nome).IsModified = true;
                _context.Entry(produto).Property(x => x.Descricao).IsModified = true;
                _context.Entry(produto).Property(x => x.UpdatedAt).IsModified = true;

                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao alterar produto");
            }
            
        }
    }
}
