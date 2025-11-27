using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.CambiarContrasenaDTO
{
    public class CambiarContrasenaDto
    {
        public int IdUsuario { get; set; }
        public string ContrasenaActual { get; set; }
        public string NuevaContrasena { get; set; }
    }

}
