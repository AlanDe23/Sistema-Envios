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
        Task<BalanceAdmin> GetUltimoBalancePorAdmin(int idUsuario);
        Task<BalanceAdmin> GetBalanceActualAsync();
        Task ResetBalanceAsync();
        Task<BalanceAdmin?> GetByDeliveryAsync(int idDelivery);
        Task AgregarAsync(BalanceAdmin balance);


        Task SoftDeleteAsync(int id);

    }

}
