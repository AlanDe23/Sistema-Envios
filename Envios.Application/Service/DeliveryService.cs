using Envios.Application.Service.Interface;
using Envios.Domain.DTOs.DeliveryDTO;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;

namespace Envios.Application.Services
{

    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<GetDeliveryResumenDto> CrearDeliveryAdminAsync(CreateDeliveryAdminDto dto)
        {
            var delivery = new Delivery
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre,              
                Telefono = dto.Telefono,
                Estado = dto.Estado,
                BalanceAcumulado = 0
            };

            await _deliveryRepository.AgregarAsync(delivery);

            return new GetDeliveryResumenDto
            {
                IdDelivery = delivery.IdDelivery,
                Telefono = delivery.Telefono,
                Estado = delivery.Estado
            };
        }

        public async Task<IEnumerable<GetDeliveryResumenDto>> GetAllAsync()
        {
            var list = await _deliveryRepository.GetAllAsync();

            return list.Select(d => new GetDeliveryResumenDto
            {
                IdDelivery = d.IdDelivery,
                Telefono = d.Telefono,
                Estado = d.Estado,
                NombreUsuario = d.Usuario != null ? d.Usuario.Nombre : string.Empty
            });
        }


        public async Task UpdateAdminAsync(UpdateDeliveryAdminDto dto)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(dto.IdDelivery);
            if (delivery == null) throw new Exception("Delivery no encontrado");

            delivery.Telefono = dto.Telefono;
            delivery.Estado = dto.Estado;
            delivery.Nombre = dto.Nombre;

            if (delivery.Usuario != null)
            {
                delivery.Usuario.Nombre = dto.Nombre;
            }

            await _deliveryRepository.ActualizarAsync(delivery);
        }


        public async Task UpdateAsync(UpdateDeliveryDto dto)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(dto.IdDelivery);
            if (delivery == null) throw new Exception("Delivery no encontrado");

            delivery.Telefono = dto.Telefono;

            await _deliveryRepository.ActualizarAsync(delivery);
        }
    }
}
