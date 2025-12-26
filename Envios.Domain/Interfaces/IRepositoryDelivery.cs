using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    

   
        public interface IDeliveryRepository : IRepositorioGenerico<Delivery>
        {
            Task<Delivery> GetByUserIdAsync(int idUsuario);

         
             Task Activar(int id);
             Task Desactivar(int id);
             Task<Delivery?> GetByUsuarioIdAsync(int idUsuario);


             Task<IEnumerable<Delivery>> GetBySucursalAsync(int idSucursal);
             Task<Delivery> GetByIdAndSucursalAsync(int idDelivery, int idSucursal);

    }


}
