using Envios.Application.DTOs.SucursalDTO;
using Envios.Application.DTOs.SucursalDTP;
using Envios.Application.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {

        private readonly ISucursalService _sucursalService;

        public SucursalController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] SucursalCrearDTO dto)
        {
            await _sucursalService.CrearSucursalAsync(dto);
            return Ok(new { mensaje = "Sucursal creada correctamente" });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
        {
            var sucursales = await _sucursalService.ObtenerPorUsuarioAsync(usuarioId);
            return Ok(sucursales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var sucursal = await _sucursalService.ObtenerPorIdAsync(id);

            if (sucursal == null)
                return NotFound();

            return Ok(sucursal);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] SucursalActualizarDTO dto)
        {
            await _sucursalService.ActualizarSucursalAsync(dto);
            return Ok(new { mensaje = "Sucursal actualizada correctamente" });
        }

        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _sucursalService.DesactivarSucursalAsync(id);
            return Ok(new { mensaje = "Sucursal desactivada" });
        }


        [HttpPut("activar/{id}")]
        public async Task<IActionResult> Activar(int id)
        {
            await _sucursalService.ActivarSucursalAsync(id);
            return Ok(new { mensaje = "Sucursal activada correctamente" });
        }

    }
}

