using Envios.Application.DTOs.PedidoDTO.Delivery;
using Envios.Application.Service.Interface;
using Envios.Domain.Entities;
using Envios.Domain.Enum;
using Envios.Domain.Interfaces;
using System.Linq;

namespace Envios.Application.Services
{
    public class PedidoServiceDelivery
    {
        private readonly IRepositorioPedido _pedidoRepo;
        private readonly IBalanceService _balanceService;

        public PedidoServiceDelivery(IRepositorioPedido pedidoRepo ,IBalanceService balanceService)
        {
            _pedidoRepo = pedidoRepo;
            _balanceService = balanceService;
        }

        public async Task CambiarEstadoEnTransitoAsync(UpdatePedidoEnTransitoDto dto , int idSucursal) 
        { 
            var pedido = await _pedidoRepo.GetByIdAndSucursalAsync(dto.IdPedido , idSucursal);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            pedido.Estado = EstadoPedido.EnTransito.ToString();
            await _pedidoRepo.ActualizarAsync(pedido);
        }

        public async Task CambiarEstadoEntregadoAsync(UpdatePedidoEntregadoDto dto , int idSucursal)
        {
            var pedido = await _pedidoRepo.GetByIdAndSucursalAsync(dto.IdPedido , idSucursal);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            if (!pedido.IdDelivery.HasValue)
                throw new Exception("Este pedido no está asignado a ningún delivery");

            pedido.Estado = EstadoPedido.Entregado.ToString();
            pedido.FechaEntrega = dto.FechaEntrega;
            pedido.MetodoPago = dto.MetodoPago.ToString();



            await _pedidoRepo.ActualizarAsync(pedido);


            //  Sincroniza el balance automaticamente
            await _balanceService.ActualizarBalanceDeliveryAsync(pedido.IdDelivery.Value , idSucursal);
        }

        public async Task CambiarEstadoNoEntregadoAsync(UpdatePedidoNoEntregadoDto dto , int IdSucursal)
        {
            var pedido = await _pedidoRepo.GetByIdAndSucursalAsync(dto.IdPedido , IdSucursal);
            if (pedido == null)
                throw new Exception("Pedido no encontrado");

            if (string.IsNullOrEmpty(dto.NotaNoEntregado))
                throw new Exception("Debe especificar una nota cuando el pedido no es entregado");

            pedido.Estado = EstadoPedido.NoEntregado.ToString();
            pedido.NotaNoEntregado = dto.NotaNoEntregado;

            await _pedidoRepo.ActualizarAsync(pedido);
        }

        /// <summary>
        /// Devuelve el resumen de pedidos para el admin.
        /// </summary>
        public async Task<object> ObtenerResumenPedidosAsync(DateTime fecha , int idSucursal)
        {
            var pedidos = await _pedidoRepo.GetPedidosPorFecha(fecha);

            var entregados = pedidos.Where(p => p.Estado == EstadoPedido.Entregado.ToString()).ToList();

            if (!entregados.Any())
                return new { mensaje = "No hay pedidos entregados en esta fecha" };

            // Verificamos si todos fueron por transferencia
            bool todosTransferencia = entregados.All(p => p.MetodoPago == MetodoPago.Transferencia.ToString() );

            if (todosTransferencia)
            {
                decimal totalEnvios = entregados.Sum(p => p.PrecioEnvio);
                return new
                {
                    mensaje = "Todos los pedidos fueron por transferencia",
                    TotalEnvios = totalEnvios,
                    Pedidos = entregados.Select(p => new
                    {
                        p.IdPedido,
                        p.PrecioPedido,
                        p.PrecioEnvio
                    })
                };
            }
            else
            {
                return new
                {
                    mensaje = "Pedidos mixtos",
                    Pedidos = entregados.Select(p => new
                    {
                        p.IdPedido,
                        p.MetodoPago,
                        PrecioPedido = p.MetodoPago.ToString() == MetodoPago.Efectivo.ToString() ? p.PrecioPedido : p.PrecioPedido - p.PrecioEnvio,
                        PrecioEnvio = p.MetodoPago.ToString() == MetodoPago.Efectivo.ToString() ? 0 : p.PrecioEnvio
                    })
                };
            }
        }

        public async Task<object> ObtenerBalanceDeliveryAsync(int idDelivery , int idSucursal)
        {
            var pedidos = await _pedidoRepo.GetAllBySucursalAsync( idSucursal);

            // 🔹 Filtrar solo pedidos entregados del delivery
            var entregados = pedidos
                .Where(p => p.IdDelivery == idDelivery && p.Estado == EstadoPedido.Entregado.ToString())
                .ToList();

            if (!entregados.Any())
                return new { mensaje = "No hay pedidos entregados para este delivery" };

            decimal totalEfectivo = entregados
                .Where(p => p.MetodoPago == MetodoPago.Efectivo.ToString())
                .Sum(p => p.PrecioPedido);

            decimal totalTransferencias = entregados
                .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
                .Sum(p => p.PrecioPedido);

            decimal totalEnviosTransferencias = entregados
                .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
                .Sum(p => p.PrecioEnvio);

            decimal totalEfectivoNeto = totalEfectivo - totalEnviosTransferencias;



            return new
            {   
                IdDelivery = idDelivery,
                TotalPedidosEntregados = entregados.Count,
                TotalEfectivo = totalEfectivo, 
                TotalEfectivoNeto = totalEfectivoNeto,                                          
                TotalTransferencias = totalTransferencias,    
                TotalEnviosTransferencias = totalEnviosTransferencias, 
                Pedidos = entregados.Select(p => new
                {
                    p.IdPedido,
                    p.PrecioPedido,
                    p.PrecioEnvio,
                    p.MetodoPago,
                    p.Estado,
                
                })
            };
        }


        public async Task<object> ObtenerPedidosPorDeliveryAsync(int idDelivery , int idSucursal)
        {
            var pedidos = await _pedidoRepo.GetAllBySucursalAsync(idSucursal);


            var pedidosDelivery = pedidos
       .Where(p => p.IdDelivery == idDelivery)
       .ToList();

            if (!pedidos.Any())
                return new { mensaje = "Este delivery no tiene pedidos asignados" };

            return pedidos.Select(p => new
            {
                p.IdPedido,
                p.FechaCreacion,
                p.Estado,
                p.MetodoPago,
                p.PrecioPedido, 
                p.PrecioEnvio,
                p.Descripcion,
                p.NombreDelcliente,
                p.NotaNoEntregado,
                MontoPorCabrar = p.PrecioPedido + p.PrecioEnvio

            });
        }

    }



}
