using EFCore;
using Library.DTO;
using Library.Repository;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.ResponseDTO.Clientess;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.BLL
{
    public class ProdutoService
    {
        private readonly ProdutoRepository _repository;

        public ProdutoService(ProdutoRepository repository)
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

        public async Task<ProdutoDetailsResponse> Buscar(string id)
        {
            Produto? produto = await _repository.Buscar(id);

            if (produto is null)
                throw new CustomException("Não encontrado", HttpStatusCode.BadRequest);

            return new ProdutoDetailsResponse(produto);
        }

        public void AtualizarPreco(decimal preco)
        {
            if (preco <= 0)
                throw new CustomException("Preço inválido", HttpStatusCode.BadRequest);       
        }      

        public void AtualizarEstoque(int quantidade)
        {
             
        }

        public async Task AtivarOuDesativar(string id, bool acao)
        {
            if (!_repository.Exists(id))
                throw new CustomException("Não encontrado", HttpStatusCode.BadRequest);

            await _repository.AtivarOuDesativar(id, acao);
        }


        private static void Validar(Produto produto) 
        {
            if (string.IsNullOrWhiteSpace(produto.Nome))
                throw new CustomException("Nome obrigatório", HttpStatusCode.BadRequest);

            if (string.IsNullOrWhiteSpace(produto.Descricao))
                throw new CustomException("Descricao obrigatório", HttpStatusCode.BadRequest);

            if (produto.Preco <= 0)
                throw new CustomException("Preço inválido", HttpStatusCode.BadRequest);

            if (produto.EstoqueDisponivel < 0)
                throw new CustomException("Estoque inválido", HttpStatusCode.BadRequest);
        }      
    }
}
