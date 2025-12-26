using Envios.Application.DTOs.BalanceDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.Service.Interface
{
    public interface IBalanceService
    {
 
            Task<bool> MarcarComoPagado(int idBalance , int idSucursal) ;
             Task<object> ObtenerBalanceDeliveryAsync(int idDelivery , int idSucursal);
             Task<bool> EliminarBalanceAsync(int idBalance , int idSucursal) ;
            Task ActualizarBalanceDeliveryAsync(int idDelivery , int idSucursal);
    }
}
