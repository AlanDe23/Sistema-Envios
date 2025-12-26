using Envios.Application.DTOs.PedidoDTO.Admin;
using Envios.Application.Service.Interface;
using Envios.Domain.DTOs.PedidoDTO;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using System.Net.Http;

namespace Envios.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IRepositorioPedido _repositorioPedido;
        private readonly IBalanceService _balanceService; // 👈 Agregado

        public PedidoService(IRepositorioPedido repositorioPedido, IBalanceService balanceService)
        {
            _repositorioPedido = repositorioPedido;
            _balanceService = balanceService;
        }

        public async Task<Pedido> CrearPedidoAsync(   CreatePedidoDto dto , int idsucursal)
        {


            var pedido = new Pedido
            {
                NombreDelcliente = dto.NombreDelcliente,
                Descripcion = dto.Descripcion,
                PrecioPedido = dto.PrecioPedido,
                PrecioEnvio = dto.PrecioEnvio,
                MetodoPago = dto.MetodoPago,
                IdSucursal = idsucursal,
                Estado = "Pendiente"
            };

            await _repositorioPedido.AgregarAsync(pedido);
            return pedido;
        }

        public async Task<Pedido> AsignarPedidoAsync(AsignarPedidoDto dto , int idSucursal)
        {
            var pedido = await _repositorioPedido.GetByIdAndSucursalAsync(dto.IdPedido, idSucursal);

            if (pedido == null)
                throw new Exception($"No se encontró el pedido con Id {dto.IdPedido}");

            pedido.IdDelivery = dto.IdDelivery;
            pedido.Estado = "Pendiente";

            await _repositorioPedido.ActualizarAsync(pedido);

            return pedido;
        }

        public async Task<Pedido> UpdatePedidoAdminAsync(UpdatePedidoAdminDto dto  , int idSucursal)
        {
            var pedido = await _repositorioPedido.GetByIdAndSucursalAsync(dto.IdPedido ,  idSucursal);

            if (pedido == null)
                throw new Exception($"No se encontró el pedido con Id {dto.IdPedido}");

            pedido.Descripcion = dto.Descripcion;
            pedido.Estado = dto.Estado.ToString();
            pedido.PrecioPedido = dto.PrecioPedido;
            pedido.PrecioEnvio = dto.PrecioEnvio;
            pedido.MetodoPago = dto.MetodoPago.ToString();
            pedido.NombreDelcliente = dto.NombreDelcliente;

            if (pedido.GetType().GetProperty("FechaEntrega") != null)
            {
                pedido.GetType().GetProperty("FechaEntrega")!.SetValue(pedido, dto.FechaEntrega);
            }

            await _repositorioPedido.ActualizarAsync(pedido);

            return pedido;
        }

        public async Task<IEnumerable<GetPedidoResumenDto>> GetPedidosResumenAsync( int IdSucursal)
        {
            var pedidos = await _repositorioPedido.GetAllBySucursalAsync(IdSucursal);

            if (pedidos == null)
                pedidos = new List<Domain.Entities.Pedido>();

            var resumen = pedidos
                .Where(p => p != null)
                .Select(p => new GetPedidoResumenDto
                {
                    IdPedido = p.IdPedido,
                    Estado = Enum.TryParse<Envios.Domain.Enum.EstadoPedido>(p.Estado, out var estado)
                                ? estado
                                : Envios.Domain.Enum.EstadoPedido.Pendiente,
                    FechaEntrega = p.FechaEntrega,
                    FechaCreacion = p.FechaCreacion,
                    NombreDelcliente = p.NombreDelcliente,
                    Descripcion = p.Descripcion,
                    PrecioPedido = p.PrecioPedido,
                    PrecioEnvio = p.PrecioEnvio,
                    MetodoPago = p.MetodoPago,
                    NotaNoEntregado = p.NotaNoEntregado,
                    IdDelivery = p.Delivery != null ? p.Delivery.IdDelivery : (int?)null,
                    NombreDelivery = p.Delivery != null ? p.Delivery.Nombre : "Sin asignar",

                   MontoPorCabrar = p.PrecioPedido + p.PrecioEnvio
                });

            return resumen;
        }

        public async Task<bool> DeletePedidoAsync(int id , int IdSucursal )
        {
            var pedido = await _repositorioPedido.GetByIdAndSucursalAsync(id , IdSucursal);
            if (pedido == null)
                return false;

            bool eliminado = await _repositorioPedido.DeleteByIdAsync(id);

            if (eliminado && pedido.IdDelivery.HasValue)
            {
                // 👉 Recalcular el balance del delivery
                await _balanceService.ActualizarBalanceDeliveryAsync(pedido.IdDelivery.Value , IdSucursal);
            }

            return eliminado;
        }
    }
}
