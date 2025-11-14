using Envios.Domain.DTOs.UsuarioDTO;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;

namespace Envios.Application.Services
{
    public class UsuarioService
    {
        private readonly IRepositorioUsuario _usuarioRepo;
        private readonly IDeliveryRepository _deliveryRepository;

        public UsuarioService(IRepositorioUsuario usuarioRepo  , IDeliveryRepository deliveryRepository)
        {
            _usuarioRepo = usuarioRepo;
            _deliveryRepository = deliveryRepository;   
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



    }
}
