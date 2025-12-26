using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.PedidoDTO.Delivery
{
    public class UpdatePedidoEnTransitoDto
    {
        public int IdPedido { get; set; }
        public EstadoPedido Estado { get; set; } = EstadoPedido.EnTransito;
    }
}
