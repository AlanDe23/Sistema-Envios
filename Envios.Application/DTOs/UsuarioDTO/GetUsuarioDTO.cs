using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.DTOs.UsuarioDTO
{
    public class GetUsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public RolUsuario Rol { get; set; }
        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; }
    }
}
