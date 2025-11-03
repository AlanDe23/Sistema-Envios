using Envios.Application.DTOs.PedidoDTO.Admin;
using Envios.Domain.DTOs.PedidoDTO;
using Envios.Domain.Entities;

namespace Envios.Application.Service.Interface
{
    public interface IPedidoService
    {
        Task<Pedido> CrearPedidoAsync(CreatePedidoDto dto);
        Task<Pedido> AsignarPedidoAsync(AsignarPedidoDto dto);
        Task<Pedido> UpdatePedidoAdminAsync(UpdatePedidoAdminDto dto);
        Task<IEnumerable<GetPedidoResumenDto>> GetPedidosResumenAsync();

        Task<bool> DeletePedidoAsync(int id);
    }
}
