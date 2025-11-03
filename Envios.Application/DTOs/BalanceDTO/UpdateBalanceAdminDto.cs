using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.DTOs.BalanceDTO
{
    public  class UpdateBalanceAdminDto
    {
        public int IdBalance { get; set; }
        public decimal TotalMontoPedidos { get; set; }
        public decimal TotalEntregados { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
