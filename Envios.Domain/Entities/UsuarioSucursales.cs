using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class UsuarioSucursales
    {
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdSucursal { get; set; }
        public Sucursal Sucursal { get; set; }

    }


}

