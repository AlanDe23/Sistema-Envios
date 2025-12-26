using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.SucursalDTP
{
    public class SucursalCrearDTO
    {
        public string NombreSucursal { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int UsuarioId { get; set; }
    }
}
