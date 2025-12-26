using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    public interface IRepositorioPedido: IRepositorioGenerico<Pedido>
    {
        Task <IEnumerable<Pedido>> GetPedidosPorFecha(DateTime fecha);
        Task<Pedido> DeletePedidosPorFecha(DateTime fecha);
        Task<Pedido> DeletePedidoPorMes(int year, int mes);
        Task<IEnumerable<Pedido>> GetPedidosEntregadosAsync();
        Task<bool> DeleteByIdAsync(int id);
        Task AgregarAsync(IEnumerable<Pedido> pedido);

        Task<IEnumerable<Pedido>> GetAllBySucursalAsync(int idSucursal);
        Task<Pedido> GetByIdAndSucursalAsync(int idPedido,int  idSucursal) ;

        Task<IEnumerable<Pedido>> GetPedidosPorDeliveryAsync(int idDelivery );

    }
}
 