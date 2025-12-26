using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        [ForeignKey("Delivery")]    
        public int? IdDelivery { get; set; }
        public string NombreDelcliente { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal PrecioEnvio { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now; 
        public string? NotaNoEntregado { get; set; }
        public DateTime? FechaEntrega { get; set; }

        public Delivery Delivery { get; set; }


        public bool IsDeleted { get; set; } = false;


        public int IdSucursal { get; set; }
        public Sucursal Sucursal { get; set; }



    }

}
