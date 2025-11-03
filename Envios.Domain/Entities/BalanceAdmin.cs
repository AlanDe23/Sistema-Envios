using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class BalanceAdmin
    {
            [Key]
            public int IdBalance { get; set; }

            [ForeignKey("IdDelivery")] 
            public int IdDelivery { get; set; }
            public decimal TotalTransferencias { get; set; }
            public decimal TotalEfectivoBruto { get; set; }
            public decimal TotalEnviosTransferencias { get; set; }
            public decimal TotalEfectivoNeto { get; set; }
            public decimal TotalFinalAdmin { get; set; }
            public int TotalPedidosEntregados { get; set; }
            public DateTime FechaActualizacion { get; set; }
            public bool Pagado { get; set; } // si se marcó como pagado
            public decimal TotalMontoPedidos { get; set; }   //  Agregar esto
            public int TotalEntregados { get; set; }
            public Delivery Delivery { get; set; } = null!;


           public bool IsDeleted { get; set; } = false;

    }
}
