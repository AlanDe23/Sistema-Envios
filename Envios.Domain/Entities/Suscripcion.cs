using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Suscripcion
    {
        public int   Id { get; set; }
        public int  UsuarioId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Activa, Expirada, Cancelada
        public string MetodoPago { get; set; }
        public string TransaccionId { get; set; }
    }
}
