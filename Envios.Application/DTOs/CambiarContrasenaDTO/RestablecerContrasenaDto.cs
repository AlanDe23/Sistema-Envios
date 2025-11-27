using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.CambiarContrasenaDTO
{

    public class RestablecerContrasenaDto
    {
        public string Token { get; set; }
        public string NuevaContrasena { get; set; }
    }
}
