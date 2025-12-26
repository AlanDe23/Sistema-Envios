using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.PedidoDTO.Admin
{
    public  class UpdatePedidoAdminDto
    {

        public int IdPedido { get; set; }
        public string Descripcion { get; set; }
        public EstadoPedido Estado { get; set; }

        public decimal PrecioPedido { get; set; }
        public decimal PrecioEnvio { get; set; }
        public MetodoPago MetodoPago { get; set; }

        public string NombreDelcliente { get; set; }

        public DateTime? FechaEntrega { get; set; } // 👈 agregado
    }
}
