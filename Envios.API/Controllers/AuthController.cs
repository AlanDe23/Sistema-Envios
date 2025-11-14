using Envios.Application.Services;
using Envios.Domain.DTOs.Login;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
    {
        try
        {
            if (dto == null)
                return BadRequest(new { message = "Los datos de inicio de sesión son requeridos." });

            if (string.IsNullOrWhiteSpace(dto.Correo))
                return BadRequest(new { message = "El correo electrónico es obligatorio." });

            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                return BadRequest(new { message = "La contraseña es obligatoria." });

            var usuario = await _authService.LoginAsync(dto.Correo, dto.Contrasena );

            return Ok(new
            {
                mensaje = $"¡Bienvenido, {usuario.Nombre}!",
                idUsuario = usuario.IdUsuario,
                nombre = usuario.Nombre,
                correo = usuario.Correo,
                rol = usuario.Rol.ToString()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}