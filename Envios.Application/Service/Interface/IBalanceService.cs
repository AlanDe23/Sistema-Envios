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
 
            Task<bool> MarcarComoPagado(int idBalance);
             Task<object> ObtenerBalanceDeliveryAsync(int idDelivery);
             Task<bool> EliminarBalanceAsync(int idBalance);
            Task ActualizarBalanceDeliveryAsync(int idDelivery);
    }
}
