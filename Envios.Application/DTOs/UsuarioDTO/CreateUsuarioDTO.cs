using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.DTOs.UsuarioDTO
{
    public class CreateUsuarioDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public RolUsuario Rol { get; set; }
    }
}
