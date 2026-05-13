using EFCore;
using Library.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.BLL
{
    public class ProdutoBLL 
    {
        private readonly ContextEFCore _context;

        public string Nome { get; private set; } = string.Empty;

        public string Descricao { get; private set; } = string.Empty;

        public decimal Preco { get; private set; }

        public int EstoqueDisponivel { get; private set; }

        public bool Ativo { get; private set; } = true;   

        public ProdutoBLL() 
        { 
            _context = new ContextEFCore(); 
        }

        public ProdutoBLL(ContextEFCore context)
        {
            _context = context;
        }

        public ProdutoBLL(
            string nome,
            string descricao,
            decimal preco,
            int estoque)
        {
            Validar(nome, preco, estoque);

            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            EstoqueDisponivel = estoque;
        }

        public void AtualizarPreco(decimal preco)
        {
            if (preco <= 0)
                throw new ArgumentException("Preço inválido");       
        }

       

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade inválida");

            if (EstoqueDisponivel < quantidade)
                throw new InvalidOperationException("Estoque insuficiente");

            EstoqueDisponivel -= quantidade;

           
        }    

        private static void Validar(
            string nome,
            decimal preco,
            int estoque)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome obrigatório");

            if (preco <= 0)
                throw new ArgumentException("Preço inválido");

            if (estoque < 0)
                throw new ArgumentException("Estoque inválido");
        }

        
    }
}
