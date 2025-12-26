using Envios.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario {  get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public RolUsuario Rol {  get; set; }
        public DateTime FechaRegistro { get; set; }

        public string? TokenRecuperacion { get; set; }
        public DateTime? TokenExpira { get; set; }


        public bool Activo { get; set; } = false;

        public ICollection<Delivery> Deliverys { get; set; }


        public ICollection<Sucursal> Sucursales { get; set; }


        public ICollection<UsuarioSucursales> UsuarioSucursales { get; set; }


    }
}
