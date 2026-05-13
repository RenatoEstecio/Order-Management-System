using EFCore;
using Library.UTIL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.BLL
{
    public class ClienteBLL 
    {
        Cliente cliente;
        
        public ClienteBLL(string nome, string email, string documento)
        {           
            cliente = new Cliente();
            cliente.Nome = nome;
            cliente.Email = email;
            cliente.Documento = documento;
        }

        public ClienteBLL(Cliente cliente)
        {          
            this.cliente = cliente;
        }

        public void Desativar()
        {
            cliente.Ativo = false;          
        }

     
        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                throw new ArgumentException("Nome é obrigatório");

            if (!EmailValido(cliente.Email))
                throw new ArgumentException("E-mail inválido");

            if (string.IsNullOrWhiteSpace(cliente.Documento))
                throw new ArgumentException("Documento obrigatório");

            if (!DocumentoValido(cliente.Documento))
                throw new ArgumentException("CPF/CNPJ inválido");
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
            documento = Regex.Replace(documento, @"\D", "");

            if (documento.Length == 11)
                return CpfValidator.Validar(documento);

            if (documento.Length == 14)
                return CpfValidator.Validar(documento);

            return false;
        }
    }
}
