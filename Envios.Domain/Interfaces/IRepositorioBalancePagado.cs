
using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    public  interface IRepositorioBalancePagado:IRepositorioGenerico<BalancePagado>
    {
        Task<IEnumerable<BalancePagado>> GetBySucursalAsync(int idSucursal);



    }
}
