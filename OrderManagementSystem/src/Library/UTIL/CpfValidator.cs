using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.UTIL
{
    public static class CpfValidator
    {
        public static bool Validar(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCpf = cpf[..9];

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}
