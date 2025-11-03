using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.DTOs.DeliveryDTO
{
    public  class CreateDeliveryAdminDto
    {

        public int IdUsuario { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; } = true;

        public string Nombre { get; set; }

    }
}
