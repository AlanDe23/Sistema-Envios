using Envios.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Envios.Domain.Entities;
using Envios.Domain.Enum;

public class AuthService
{
    private readonly IRepositorioUsuario _usuarioRepo;
    private readonly IDeliveryRepository _deliveryRepo;

    public AuthService(IRepositorioUsuario usuarioRepo , IDeliveryRepository deliveryRepo)
    {
        _usuarioRepo = usuarioRepo;
        _deliveryRepo = deliveryRepo;
            

    }

    public async Task<Usuario> LoginAsync(string correo, string contrasena )
    {
       // try
    //    {
            var usuario = await _usuarioRepo.GetByEmailAsync(correo);
            

            if (usuario == null)
                throw new Exception("El correo ingresado no está registrado.");

            if (usuario.Contrasena != contrasena)
                throw new Exception("La contraseña es incorrecta.");

            // 🚨 Nueva validación: ¿está activo?
            if (!usuario.Activo)
                throw new Exception("Tu cuenta está desactivada. Contacta al administrador.");

            // 🔥 SOLO validar Delivery si es usuario de tipo Delivery
            if (usuario.Rol == RolUsuario.Delivery)
            {
                var delivery = await _deliveryRepo.GetByIdAsync(usuario.IdUsuario);

                if (delivery == null)
                    throw new Exception("No existe un perfil de delivery asociado a este usuario.");

                if (!delivery.Activo)
                    throw new Exception("Tu cuenta de delivery está desactivada. Contacta al administrador.");
            }

            // 🔥 Si todo está bien retorna el usuario
            return usuario;
        }
     //   catch (Exception ex)
     //   {
      //      Console.WriteLine($"[ERROR LOGIN]: {ex.Message}");
       //     throw;
       // }
    }
