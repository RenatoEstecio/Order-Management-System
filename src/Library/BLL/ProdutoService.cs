using EFCore;
using Library.DTO;
using Library.Repository;
using Library.RequestDTO;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.BLL
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProdutoListAllResponse> Listar(string query, int quantidade, int page)
        {
            if (quantidade > 20 || quantidade < 1)
                throw new CustomException("Quantidade inválida", HttpStatusCode.BadRequest);

            if (page < 1)
                throw new CustomException("Página inválida", HttpStatusCode.BadRequest);

            var result = await _repository.Listar(query, quantidade, page);

            if (result.Produtos.Count == 0)
                throw new CustomException("Nenhum Resultado Encontrado", HttpStatusCode.NotFound);

            return result;
        }
        public async Task<Produto> Criar(ProdutoRequest request)
        {
            Produto produto = new Produto();
            produto.Nome = request.Nome.Trim();
            produto.EstoqueDisponivel = request.Estoque;
            produto.Preco = MoneyHelper.Round(request.Preco);
            produto.Descricao = request.Descricao.Trim();
            produto.CreatedAt = produto.UpdatedAt = DateTimeHelper.ToSaoPaulo();
            produto.Ativo = true;

            Validar(produto);

            return await _repository.Criar(produto);
        }

        public async Task<Produto> Alterar(ProdutoUpdateRequest request, string id)
        {
            Produto? produto = await _repository.Buscar(id);

            if (produto == null)
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);

            if (request == null || (request.Nome == null && request.Descricao == null))
                throw new CustomException("Dados inválidos", HttpStatusCode.BadRequest);

            if(request.Nome is not null)
                produto.Nome = request.Nome.Trim();

            if (request.Descricao is not null)
                produto.Descricao = request.Descricao.Trim();

            produto.UpdatedAt = DateTimeHelper.ToSaoPaulo();
            
            Validar(produto, false);

            return await _repository.Alterar(produto);
        }


        public async Task<ProdutoDetailsResponse> Buscar(string id)
        {
            Produto? produto = await _repository.Buscar(id);

            if (produto is null)
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);

            return new ProdutoDetailsResponse(produto);
        }

        public void AtualizarPreco(decimal preco)
        {
            if (preco <= 0)
                throw new CustomException("Preço inválido", HttpStatusCode.BadRequest);       
        }      
        
        public async Task AtivarOuDesativar(string id, bool acao)
        {
            if (!_repository.Exists(id))
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);

            await _repository.AtivarOuDesativar(id, acao);
        }

        public async Task AtualizarEstoque(string id, int quantidade)
        {
            if (quantidade == 0)
                throw new CustomException("Quantidade inválida", HttpStatusCode.BadRequest);

            Produto? produto = await _repository.Buscar(id);

            if (produto == null)
                throw new CustomException("Não encontrado", HttpStatusCode.NotFound);          

            if (!(produto.EstoqueDisponivel + quantidade >= 0))
                throw new CustomException($"Restam {produto.EstoqueDisponivel} unidade(s)", HttpStatusCode.Conflict);

            bool result = await _repository.AtualizarEstoque(id, quantidade);

            if(!result)
                throw new CustomException($"Falha ao atualizar estoque", HttpStatusCode.InternalServerError);
        }


        private static void Validar(Produto produto, bool criacao = true) 
        {
            if (string.IsNullOrWhiteSpace(produto.Nome))
                throw new CustomException("Nome obrigatório", HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                throw new CustomException("Descricao obrigatório", HttpStatusCode.BadRequest);

            if (criacao)
            {
                if (produto.Preco <= 0)
                    throw new CustomException("Preço inválido", HttpStatusCode.BadRequest);

                if (produto.EstoqueDisponivel <= 0)
                    throw new CustomException("Estoque inválido", HttpStatusCode.BadRequest);
            }
        }      
    }
}
