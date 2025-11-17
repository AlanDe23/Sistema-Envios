using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Sucursal
    {
        [Key]
        public int IdSucursal { get; set; }

        public string NombreSucursal { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        // FK hacia Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Estado (soft delete y control de activación)
        public bool Activa { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
