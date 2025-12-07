using Envios.Application.DTOs;
using Envios.Application.DTOs.SuscripcionDTO;
using Envios.Application.Service.Interface;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;

namespace Envios.Application.Services
{
    public class SuscripcionService
    {
        private readonly IRepositorioSuscripcion _repo;
        private readonly IPagoService _pagoService;

        public SuscripcionService(IRepositorioSuscripcion repo, IPagoService pagoService)
        {
            _repo = repo;
            _pagoService = pagoService;
        }

        public async Task<Suscripcion> CrearSuscripcionAsync(CrearSuscripcionDTO dto)
        {
            var respuestaPago = await _pagoService.ProcesarPagoAsync(dto);

            var suscripcion = new Suscripcion
            {
                Id = dto.UsuarioId,
                UsuarioId = dto.UsuarioId,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddMonths(1),
                Monto = dto.Monto,
                MetodoPago = dto.MetodoPago,
                Estado = respuestaPago.Exitoso ? "Activa" : "Pendiente",
                TransaccionId = respuestaPago.TransaccionId
            };

            await _repo.CrearAsync(suscripcion);
            return suscripcion;
        }
    }
}
