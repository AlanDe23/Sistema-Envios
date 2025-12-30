using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Suscripcion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? IdPlan { get; set; }

        [Column("CarnetSubscriptionId")]
        [MaxLength(100)]
        public string CarnetSubscriptionId { get; set; } // ← TransaccionId
        public string Estado { get; set; }               // Activa, Suspendida
        public decimal Monto { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public bool RenovacionAutomatica { get; set; }
        public string MetodoPago { get; set; }           // Carnet
        public string TransaccionId { get; set; }
    }

}
