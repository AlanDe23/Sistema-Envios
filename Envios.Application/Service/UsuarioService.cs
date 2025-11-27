using Envios.Application.DTOs.CambiarContrasenaDTO;
using Envios.Application.Service.Envios.Application.Services;
using Envios.Domain.DTOs.UsuarioDTO;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using System.Collections.Concurrent;

namespace Envios.Application.Services
{
    public class UsuarioService
    {
        private readonly IRepositorioUsuario _usuarioRepo;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly EmailService _emailService;
        private static readonly ConcurrentDictionary<string, (int userId, DateTime expiration)> _tokens
       = new ConcurrentDictionary<string, (int, DateTime)>();


        public UsuarioService(IRepositorioUsuario usuarioRepo  , IDeliveryRepository deliveryRepository , EmailService emailService)
        {
            _usuarioRepo = usuarioRepo;
            _deliveryRepository = deliveryRepository;
            _emailService = emailService;
        }

        public async Task<GetUsuarioDto> CreateAsync(CreateUsuarioDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Contrasena = dto.Contrasena,
                Rol = dto.Rol,
                FechaRegistro = DateTime.UtcNow
            };

            await _usuarioRepo.AgregarAsync(usuario);

            return new GetUsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<GetUsuarioDto?> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(id);
            if (usuario == null) return null;

            return new GetUsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task<IEnumerable<GetUsuarioDto>> GetAllAsync()
        {
            var usuarios = await _usuarioRepo.GetAllAsync();
            return usuarios.Select(u => new GetUsuarioDto
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Correo = u.Correo,
                Rol = u.Rol,
                FechaRegistro = u.FechaRegistro
            });
        }

        public async Task<bool> UpdateAsync(UpdateUsuarioDto dto)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(dto.IdUsuario);
            if (usuario == null) return false;

            usuario.Nombre = dto.Nombre;
            usuario.Correo = dto.Correo;
            usuario.Rol = dto.Rol;

            if (!string.IsNullOrWhiteSpace(dto.Contrasena))
            {
                usuario.Contrasena = dto.Contrasena;
            }

            await _usuarioRepo.ActualizarAsync(usuario);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(id);
            if (usuario == null)
                return false;

            // Eliminar todos los Delivery asociados al usuario
            var deliveries = await _deliveryRepository.GetAllAsync();
            var deliveriesUsuario = deliveries.Where(d => d.IdUsuario == id).ToList();

            foreach (var delivery in deliveriesUsuario)
            {
                await _deliveryRepository.EliminarAsync(delivery);
            }

            // Ahora sí eliminamos el usuario
            await _usuarioRepo.EliminarAsync(usuario);
            return true;
        }


        public async Task ActivarUsuario(int id)
        {
            var user = await _usuarioRepo.GetByIdAsync(id);
            user.Activo = true;
            await _usuarioRepo.ActualizarAsync(user);
        }

        public async Task DesactivarUsuario(int id)
        {
            var user = await _usuarioRepo.GetByIdAsync(id);
            user.Activo = false;
            await _usuarioRepo.ActualizarAsync(user);
        }

        public async Task CambiarContrasenaAsync(CambiarContrasenaDto dto)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(dto.IdUsuario);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (usuario.Contrasena != dto.ContrasenaActual)
                throw new Exception("La contraseña actual no es correcta");

            usuario.Contrasena = dto.NuevaContrasena;

            await _usuarioRepo.ActualizarAsync(usuario);
        }


        public async Task<string> SolicitarRecuperacionAsync(string correo)
        {
            var usuario = await _usuarioRepo.GetByEmailAsync(correo);

            if (usuario == null)
                return "";

            string token = Guid.NewGuid().ToString();
            DateTime expiracion = DateTime.UtcNow.AddMinutes(30);

            _tokens[token] = (usuario.IdUsuario, expiracion);

            // 🔹 Enviar el correo real
            string mensaje = $@"
        Hola {usuario.Nombre},<br><br>
        Usa este token para restablecer tu contraseña:<br>
        <b>{token}</b><br><br>
        Este token expira en 30 minutos.
    ";

            await _emailService.EnviarCorreoAsync(usuario.Correo, "Recuperación de Contraseña", mensaje);

            return token;
        }

        public async Task RestablecerContrasenaAsync(RestablecerContrasenaDto dto)
        {
            if (!_tokens.TryGetValue(dto.Token, out var data))
                throw new Exception("Token inválido");

            if (DateTime.UtcNow > data.expiration)
            {
                _tokens.TryRemove(dto.Token, out _);
                throw new Exception("Token expirado");
            }

            var usuario = await _usuarioRepo.GetByIdAsync(data.userId);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Contrasena = dto.NuevaContrasena;
            await _usuarioRepo.ActualizarAsync(usuario);

            _tokens.TryRemove(dto.Token, out _);
        }

    }
}
