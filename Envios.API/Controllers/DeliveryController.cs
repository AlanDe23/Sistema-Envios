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
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            var result = await _deliveryService.CrearDeliveryAdminAsync(dto , idSucursal);
            return Ok(result);
        }

        // Obtener todos los deliveries
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            var result = await _deliveryService.GetAllAsync(idSucursal);
            return Ok(result);
        }

        // Actualizar delivery completo (admin)
        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateDeliveryAdminDto dto)
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            await _deliveryService.UpdateAdminAsync(dto , idSucursal);
            return NoContent();
        }

        // Actualizar delivery básico (ej. teléfono)
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateDeliveryDto dto)
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            await _deliveryService.UpdateAsync(dto , idSucursal);
            return NoContent();
        }



        [HttpPatch("delivery/{id}/activar")]
        public async Task<IActionResult> ActivarDelivery(int id)
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];

            await _deliveryService.Activar(id , idSucursal );
            return Ok("Delivery activado");
        }

        [HttpPatch("delivery/{id}/desactivar")]
        public async Task<IActionResult> DesactivarDelivery(int id)
        {
            int idSucursal = (int)HttpContext.Items["IdSucursal"];
            await _deliveryService.Desactivar(id , idSucursal);
            return Ok("Delivery desactivado");
        }

    }
}
    