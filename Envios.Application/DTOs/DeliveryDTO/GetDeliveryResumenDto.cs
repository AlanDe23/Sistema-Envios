    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Envios.Domain.DTOs.DeliveryDTO
    {
        public class GetDeliveryResumenDto
        {
            public int IdDelivery { get; set; }
            public string Telefono { get; set; }
            public bool Estado { get; set; }

            public bool Activo { get; set; }

            public string NombreUsuario { get; set; }

            public int IdSucursal { get; set; }

        }
    }
    
