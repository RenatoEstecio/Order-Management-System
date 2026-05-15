using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class HistoricoListResponse
    {
        public HistoricoListResponse() { }

        public DateTime Horario {  get; set; }          
        public string Mensagem { get; set; }
    }
}
