

namespace Envios.Application.DTOs.BalanceDTO
{
    public class BalanceDetalleDto
    {
        public decimal TotalTransferencias { get; set; }  // Todo lo que ya entró al admin
        public decimal TotalEfectivoBruto { get; set; }   // Todo lo que se cobró en efectivo
        public decimal TotalEnviosTransferencias { get; set; } // Lo que el admin debe al delivery
        public decimal TotalEfectivoNeto { get; set; }    // Efectivo que realmente recibe el admin
        public decimal TotalFinalAdmin { get; set; }      // Transferencias + EfectivoNeto
        public int TotalPedidosEntregados { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
