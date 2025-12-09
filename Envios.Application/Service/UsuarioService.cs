using Envios.Application.DTOs.CambiarContrasenaDTO;
using Envios.Application.Service.Envios.Application.Services;
using Envios.Domain.DTOs.UsuarioDTO;
using Envios.Domain.Entities;
using Envios.Domain.Enum;
using Envios.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Security.Cryptography;

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
                FechaRegistro = DateTime.Now
            };

        

            await _usuarioRepo.AgregarAsync(usuario);

            if (usuario.Rol == RolUsuario.Delivery)
            {
                
                await _emailService.EnviarBienvenidaDeliveryAsync(
                    usuario.Correo,
                    usuario.Nombre,
                    usuario.IdUsuario
                );
            }
            else
            {
     
               await _emailService.EnviarBienvenidaAsync(usuario.Correo, usuario.Nombre);
            }

       
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
                FechaRegistro = usuario.FechaRegistro,
                Activo = usuario.Activo 

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
                FechaRegistro = u.FechaRegistro,
                Activo =  u.Activo
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

            // 🔹 Generar token de 6 números
            var random = new Random();
            string token = random.Next(100000, 999999).ToString();

            DateTime expiracion = DateTime.Now.AddMinutes(30);
            usuario.TokenRecuperacion = token;
            usuario.TokenExpira = expiracion;

            await _usuarioRepo.ActualizarAsync(usuario);
            // 🔹 Email elegante en HTML
            string mensaje = $@"
    <div style='font-family: Arial; padding: 20px; background:#f4f4f4;'>
        <div style='max-width: 500px; margin: auto; background: white; padding: 25px; border-radius: 10px; 
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);'>

            <h2 style='color:#2C73D2; text-align:center;'>Recuperación de Contraseña</h2>

            <p style='font-size: 15px; color:#333;'>
                Hola <b>{usuario.Nombre}</b>,
            </p>

            <p style='font-size: 15px; color:#333;'>
                Recibimos una solicitud para restablecer tu contraseña.  
                Utiliza el siguiente código para continuar:
            </p>

            <div style='text-align:center; margin: 30px 0;'>
                <span style='font-size: 40px; letter-spacing: 10px; color:#2C73D2; font-weight: bold;'>
                    {token}
                </span>
            </div>

            <p style='font-size: 14px; color:#555; text-align:center;'>
                El código expirará en <b>30 minutos</b>.
            </p>

            <hr style='margin-top: 30px;'>

            <p style='font-size: 12px; color:#888; text-align:center;'>
                Si no solicitaste este cambio, puedes ignorar este mensaje.
            </p>
        </div>
    </div>";

            await _emailService.EnviarCorreoAsync(usuario.Correo, "Recuperación de Contraseña", mensaje);

            return token;
        }
        public async Task RestablecerContrasenaAsync(RestablecerContrasenaDto dto)
        {
            var usuario = await _usuarioRepo.GetByTokenAsync(dto.Token);
            if (usuario == null)
                throw new Exception("Token inválido");

            if (!usuario.TokenExpira.HasValue || usuario.TokenExpira < DateTime.Now)
                throw new Exception("Token expirado");

            usuario.Contrasena = dto.NuevaContrasena;
            usuario.TokenRecuperacion = null;
            usuario.TokenExpira = null;

            await _usuarioRepo.ActualizarAsync(usuario);
        }



        public async Task<string> GenerarTokenRecuperacionAsync(string correo)
        {
            var usuario = await _usuarioRepo.GetByEmailAsync(correo);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            var token = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            usuario.TokenRecuperacion = token;
            usuario.TokenExpira = DateTime.Now.AddMinutes(30);

            await _usuarioRepo.ActualizarAsync(usuario);
            return token;
        }




    }
}
