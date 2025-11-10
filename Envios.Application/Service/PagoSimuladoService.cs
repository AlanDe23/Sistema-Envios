using Envios.Application.DTOs;
using Envios.Application.DTOs.SuscripcionDTO;
using Envios.Application.Service.Interface;

namespace Envios.Infrastructure.Services
{
    public class PagoSimuladoService : IPagoService
    {
        public async Task<RespuestaPagoDTO> ProcesarPagoAsync(CrearSuscripcionDTO dto)
        {
            await Task.Delay(1000); // Simula el tiempo de respuesta del gateway
            return new RespuestaPagoDTO
            {
                Exitoso = true,
                TransaccionId = Guid.NewGuid().ToString(),
                Mensaje = "Pago procesado exitosamente (simulado)"
            };
        }
    }
}
