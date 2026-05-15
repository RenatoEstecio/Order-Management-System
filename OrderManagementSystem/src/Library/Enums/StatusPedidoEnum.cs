using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Enums
{
    public enum StatusPedido { Criado = 1, Pago = 2, Enviado = 3, Cancelado = 4 }
    public enum StatusPedidoAlteracao 
    { 
        Pago = StatusPedido.Pago, 
        Enviado = StatusPedido.Enviado, 
        Cancelado = StatusPedido.Cancelado 
    }
}
