using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    public interface IRepositorioBalanceAdmin : IRepositorioGenerico<BalanceAdmin>
    {
        Task<BalanceAdmin?> GetByIdAndSucursalAsync(int idBalance, int idSucursal);

        Task<BalanceAdmin?> GetByDeliveryAndSucursalAsync(
            int idDelivery,
            int idSucursal
        );

        Task AgregarAsync(BalanceAdmin balance);
        Task ActualizarAsync(BalanceAdmin balance);
        Task<int> GuardarCambiosAsync();

        Task SoftDeleteAsync(int idBalance, int idSucursal);
    }

}


