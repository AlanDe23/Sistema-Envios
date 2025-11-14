using Envios.Application.Service.Interface;
using Envios.Application.Services;
using Envios.Domain.DTOs.DeliveryDTO;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        // Crear Delivery
        [HttpPost("CrearDeliveryAdmin")]
        public async Task<IActionResult> CrearDeliveryAdmin([FromBody] CreateDeliveryAdminDto dto)
        {
            var result = await _deliveryService.CrearDeliveryAdminAsync(dto);
            return Ok(result);
        }

        // Obtener todos los deliveries
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _deliveryService.GetAllAsync();
            return Ok(result);
        }

        // Actualizar delivery completo (admin)
        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateDeliveryAdminDto dto)
        {
            await _deliveryService.UpdateAdminAsync(dto);
            return NoContent();
        }

        // Actualizar delivery básico (ej. teléfono)
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateDeliveryDto dto)
        {
            await _deliveryService.UpdateAsync(dto);
            return NoContent();
        }



        [HttpPatch("delivery/{id}/activar")]
        public async Task<IActionResult> ActivarDelivery(int id)
        {
            await _deliveryService.Activar(id);
            return Ok("Delivery activado");
        }

        [HttpPatch("delivery/{id}/desactivar")]
        public async Task<IActionResult> DesactivarDelivery(int id)
        {
            await _deliveryService.Desactivar(id);
            return Ok("Delivery desactivado");
        }

    }
}
