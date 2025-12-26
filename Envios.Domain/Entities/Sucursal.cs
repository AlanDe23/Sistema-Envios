using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        // ✅ FK REAL (EXISTE EN BD)
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario Usuario { get; set; }

        public bool Activa { get; set; }
        public DateTime FechaRegistro { get; set; }


        public ICollection<UsuarioSucursales> UsuarioSucursales { get; set; }
    }
}
