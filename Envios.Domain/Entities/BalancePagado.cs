using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities    
{
    public class BalancePagado
    {
        [Key]
        public int IdBalancePagado { get; set; }
        [ForeignKey("Balance")]
        public int IdBalance { get; set; }   // referencia al balance original
        public int IdDelivery { get; set; }
        public int TotalEntregados { get; set; }
        public decimal TotalMontoPedidos { get; set; }
        public DateTime FechaPago { get; set; }
        public int IdSucursal { get; set; }

        public Sucursal Sucursal { get; set; }
    }
}
