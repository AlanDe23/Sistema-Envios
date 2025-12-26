using Envios.Application.DTOs.CambiarContrasenaDTO;
using Envios.Application.Services;
using Envios.Domain.DTOs.UsuarioDTO;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
        {
            try
            {
                var result = await _usuarioService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el controlador al crear usuario: {ex.Message}");
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
           {
                var usuarios = await _usuarioService.GetAllAsync();
                return Ok(usuarios);
           }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el controlador al obtener todos los usuarios: {ex.Message}");
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest("El id de la URL no coincide con el del cuerpo de la petición.");

            var actualizado = await _usuarioService.UpdateAsync(dto);

            if (!actualizado)
                return NotFound("Usuario no encontrado.");

            return Ok("Usuario actualizado correctamente.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound(new { mensaje = $"Usuario con Id {id} no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error en el controlador al obtener usuario",
                    detalle = ex.Message
                });
            }
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
               {
                var result = await _usuarioService.DeleteAsync(id);
                if (!result) return NotFound();

                return NoContent();
                }
            catch (Exception ex)
              {
               return StatusCode(500, $"Error en el controlador al eliminar usuario: {ex.Message}");
                }
        }


        [HttpPatch("{id}/activar")]
        public async Task<IActionResult> ActivarUsuario(int id)
        {
            await _usuarioService.ActivarUsuario(id);
            return Ok("Usuario activado");
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            await _usuarioService.DesactivarUsuario(id);
            return Ok("Usuario desactivado");
        }


        [HttpPost("cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto dto)
        {
            await _usuarioService.CambiarContrasenaAsync(dto);
            return Ok("Contraseña cambiada correctamente.");
        }


        [HttpPost("recuperar")]
        public async Task<IActionResult> SolicitarRecuperacion([FromBody] RecuperarContrasenaDto dto)
        {
            var token = await _usuarioService.SolicitarRecuperacionAsync(dto.Correo);

            return Ok(new
            {
                mensaje = "Si el correo existe, se enviará un enlace de recuperación.",
                token = token // mostrarlo para pruebas (en producción se envía por email)
            });
        }


        [HttpPost("restablecer")]
        public async Task<IActionResult> Restablecer([FromBody] RestablecerContrasenaDto dto)
        {
            await _usuarioService.RestablecerContrasenaAsync(dto);
            return Ok("Contraseña restablecida correctamente.");
        }




    }
}
