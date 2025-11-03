using Envios.Application.DTOs.BalanceDTO;
using Envios.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpPut("marcar-pagado/{id}")]
        public async Task<ActionResult> MarcarPagado(int id)
        {
            var result = await _balanceService.MarcarComoPagado(id);
            if (!result) return NotFound("Balance no encontrado.");
            return Ok("Balance marcado como pagado y guardado en historial.");
        }


        // 🔹 Nuevo endpoint: balance por delivery
        [HttpGet("delivery/{idDelivery}")]
        public async Task<IActionResult> ObtenerBalanceDelivery(int idDelivery)
        {
            var balance = await _balanceService.ObtenerBalanceDeliveryAsync(idDelivery);
            return Ok(balance);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarBalance(int id)
        {
            var eliminado = await _balanceService.EliminarBalanceAsync(id);
            if (!eliminado)
                return NotFound("El balance no existe o ya fue eliminado.");

            return Ok("Balance eliminado correctamente.");
        }

    }
}
