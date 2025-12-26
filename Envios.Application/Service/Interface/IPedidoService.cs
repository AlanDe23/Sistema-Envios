using Envios.Application.DTOs.PedidoDTO.Admin;
using Envios.Domain.DTOs.PedidoDTO;
using Envios.Domain.Entities;

namespace Envios.Application.Service.Interface
{
    public interface IPedidoService
    {
        Task<Pedido> CrearPedidoAsync(CreatePedidoDto dto , int idsucursal);
        Task<Pedido> AsignarPedidoAsync(AsignarPedidoDto dto ,  int idSucursal);
        Task<Pedido> UpdatePedidoAdminAsync(UpdatePedidoAdminDto dto , int IdSucursal);
        Task<IEnumerable<GetPedidoResumenDto>> GetPedidosResumenAsync(int IdSucursal);

        Task<bool> DeletePedidoAsync(int id , int IdSucursal);
    }
}
