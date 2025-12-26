using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.PedidoDTO.Admin
{
   
   
        public class CreatePedidoDto
        {
            public string NombreDelcliente { get; set; }
            public string Descripcion { get; set; }
            public decimal PrecioPedido { get; set; }
            public decimal PrecioEnvio { get; set; }
            public string MetodoPago { get; set; }

            public string FechaCreacion { get; set; }



    }
    

}
