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
        Task<GetDeliveryResumenDto> CrearDeliveryAdminAsync(CreateDeliveryAdminDto dto , int IdSucursal);
        Task<IEnumerable<GetDeliveryResumenDto>> GetAllAsync(int IdSucursal);
        Task UpdateAdminAsync(UpdateDeliveryAdminDto dto ,  int idSurcusal ) ;
        Task UpdateAsync(UpdateDeliveryDto dto , int idSurcusal);

        Task Activar(int id, int idSurcusal);
        Task Desactivar(int id, int idSurcusal);

    }
}
