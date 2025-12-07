using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.PedidoDTO.Admin
{
    public class GetPedidoResumenDto
    {
        public string? NotaNoEntregado { get; set; }
        public int IdPedido { get; set; }
        public EstadoPedido Estado { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string NombreDelcliente { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal PrecioEnvio { get; set; }
        public int? IdDelivery { get; set; }
        public string MetodoPago { get; set; }
        public string NombreDelivery { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public decimal MontoPorCabrar { get; set; }
    }
}
