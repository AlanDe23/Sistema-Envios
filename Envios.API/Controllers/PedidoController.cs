using Envios.Application.DTOs.PedidoDTO.Admin;
using Envios.Application.Service.Interface;
using Envios.Domain.DTOs.PedidoDTO;
using Envios.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost("crear/")]
        public async Task<IActionResult> CrearPedido([FromBody] CreatePedidoDto dto)
        {
            if (dto == null)
                return BadRequest("El pedido no puede ser nulo.");

            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            var pedido = await _pedidoService.CrearPedidoAsync(dto , idSucursal);

            return CreatedAtAction(nameof(CrearPedido), new { id = pedido.IdPedido }, pedido);
        }

        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarPedido([FromBody] AsignarPedidoDto dto)
        {
            if (dto == null)
                return BadRequest("Los datos de asignación son inválidos.");

            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            var pedido = await _pedidoService.AsignarPedidoAsync(dto , idSucursal);

            return Ok(new
            {
                mensaje = $"Pedido {pedido.IdPedido} asignado al delivery {pedido.IdDelivery}",
                pedido
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePedido( [FromBody] UpdatePedidoAdminDto dto)
        {
            if (dto == null)
                return BadRequest("Los datos de actualización son inválidos.");

            int idSucursal = (int)HttpContext.Items["IdSucursal"];
            var pedido = await _pedidoService.UpdatePedidoAdminAsync(dto , idSucursal);

            return Ok(new
            {
                mensaje = $"Pedido {pedido.IdPedido} actualizado correctamente.",
                pedido
            });
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen()
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            var pedidos = await _pedidoService.GetPedidosResumenAsync(idSucursal);
            return Ok(pedidos);
        }


        // 🔹 Borrar pedido por Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id  )
        {

            int idSucursal = (int)HttpContext.Items["IdSucursal"];
            var eliminado = await _pedidoService.DeletePedidoAsync(id , idSucursal );

            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró el pedido con Id {id}" });

            return Ok(new { mensaje = $"Pedido con Id {id} eliminado correctamente." });
        }
    }
}
