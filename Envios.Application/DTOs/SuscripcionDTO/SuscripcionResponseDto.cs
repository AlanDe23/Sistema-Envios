using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.SuscripcionDTO
{
    public class SuscripcionResponseDto
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa => Estado == "Activa";
    }
}
