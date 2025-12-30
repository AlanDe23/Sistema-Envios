using Envios.Application.DTOs;
using Envios.Application.DTOs.Carnet;
using Envios.Application.DTOs.SuscripcionDTO;
using Envios.Application.Service.Interface;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;

namespace Envios.Application.Services
{
    public class SuscripcionService
    {
        private readonly IRepositorioSuscripcion _repo;
        //private readonly IPagoService _pagoService;

        public SuscripcionService(IRepositorioSuscripcion repo )
        {
            _repo = repo;
       
        }

        public async Task ProcesarWebhookCarnet(CarnetWebhookDto dto)
        {
            var suscripcion = await _repo.GetByCarnetIdAsync(dto.Subscription_Id);
            if (suscripcion == null) return;

            if (dto.Event == "payment.success")
            {
                suscripcion.Estado = "Activa";
                suscripcion.FechaInicio = DateTime.Now;
                suscripcion.FechaFin = DateTime.Now.AddMonths(1);
            }

            if (dto.Event == "payment.failed")
            {
                suscripcion.Estado = "Suspendida";
            }

            await _repo.SaveChangesAsync();
        }

    }
}

