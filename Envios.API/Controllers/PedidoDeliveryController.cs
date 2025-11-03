using Envios.Application.DTOs.PedidoDTO.Delivery;
using Envios.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoDeliveryController : ControllerBase
    {
        private readonly PedidoServiceDelivery _pedidoServiceDelivery;

        public PedidoDeliveryController(PedidoServiceDelivery pedidoServiceDelivery)
        {
            _pedidoServiceDelivery = pedidoServiceDelivery;
        }

        [HttpPut("estado/en-transito")]
        public async Task<IActionResult> CambiarAEnTransito(UpdatePedidoEnTransitoDto dto)
        {
            try
            {
                await _pedidoServiceDelivery.CambiarEstadoEnTransitoAsync(dto);
                return Ok(new { mensaje = "Pedido marcado como En Transito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al cambiar estado", detalle = ex.Message });
            }
        }

        [HttpPut("estado/entregado")]
        public async Task<IActionResult> CambiarAEntregado(UpdatePedidoEntregadoDto dto)
        {
            try
            {
                await _pedidoServiceDelivery.CambiarEstadoEntregadoAsync(dto);
                return Ok(new { mensaje = "Pedido marcado como Entregado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al cambiar estado", detalle = ex.Message });
            }
        }

        [HttpPut("estado/no-entregado")]
        public async Task<IActionResult> CambiarANoEntregado(UpdatePedidoNoEntregadoDto dto)
        {
            try
            {
                await _pedidoServiceDelivery.CambiarEstadoNoEntregadoAsync(dto);
                return Ok(new { mensaje = "Pedido marcado como No Entregado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al cambiar estado", detalle = ex.Message });
            }
        }


        [HttpGet("resumen/{fecha}")]
        public async Task<IActionResult> ObtenerResumen(DateTime fecha)
        {
            try
            {
                var resumen = await _pedidoServiceDelivery.ObtenerResumenPedidosAsync(fecha);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener resumen de pedidos",
                    detalle = ex.Message
                });
            }
        }



        [HttpGet("balance/{idDelivery}")]
        public async Task<IActionResult> ObtenerBalance(int idDelivery)
        {
            try
            {
                var balance = await _pedidoServiceDelivery.ObtenerBalanceDeliveryAsync(idDelivery);
                return Ok(balance);
            }
            catch (Exception ex)
             {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener balance del delivery",
                    detalle = ex.Message
                });
           }
        }



        [HttpGet("pedidos/{idDelivery}")]
        public async Task<IActionResult> ObtenerPedidosPorDelivery(int idDelivery)
        {
            try
            {
                var pedidos = await _pedidoServiceDelivery.ObtenerPedidosPorDeliveryAsync(idDelivery);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener los pedidos del delivery",
                    detalle = ex.Message
                });
            }
        }


    }
}
