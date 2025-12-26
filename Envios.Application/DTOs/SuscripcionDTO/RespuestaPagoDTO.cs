using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.SuscripcionDTO
{
    public class RespuestaPagoDTO
    {
        public bool Exitoso { get; set; }
        public string TransaccionId { get; set; }
        public string Mensaje { get; set; }

    }
}
