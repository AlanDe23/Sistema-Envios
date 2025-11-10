using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.SuscripcionDTO
{
    public class CrearSuscripcionDTO
    {
        public int  UsuarioId { get; set; }
        public string MetodoPago { get; set; } // Stripe, PayPal, etc.
        public decimal Monto { get; set; }
    }
}
