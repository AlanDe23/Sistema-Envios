using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.BalanceDTO
{
    public class BalancePedidoDto
    {
        public int PedidoId { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } // "Efectivo" o "Transferencia"
        public bool Entregado { get; set; }

        public decimal MontoEnvio { get; set; }
    }
}
