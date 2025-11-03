using Envios.Domain.DTOs.DeliveryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.Service.Interface
{
    public interface IDeliveryService
    {
        Task<GetDeliveryResumenDto> CrearDeliveryAdminAsync(CreateDeliveryAdminDto dto);
        Task<IEnumerable<GetDeliveryResumenDto>> GetAllAsync();
        Task UpdateAdminAsync(UpdateDeliveryAdminDto dto);
        Task UpdateAsync(UpdateDeliveryDto dto);
    }
}
