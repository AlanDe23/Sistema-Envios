using Envios.Application.DTOs;
using Envios.Application.DTOs.SuscripcionDTO;
using Envios.Application.Service.Interface;
using Envios.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuscripcionesController : ControllerBase
    {
        private readonly SuscripcionService _service;

        public SuscripcionesController(SuscripcionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearSuscripcionDTO dto)
        {
            var suscripcion = await _service.CrearSuscripcionAsync(dto);
            return Ok(suscripcion);
        }
    }
}
