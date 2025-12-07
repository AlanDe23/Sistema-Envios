using Envios.Application.DTOs.Login;
using Envios.Domain.DTOs.Login;
using Envios.Domain.Entities;
using Envios.Domain.Enum;
using Envios.Domain.Interfaces;

public class AuthService
{
    private readonly IRepositorioUsuario _usuarioRepo;
    private readonly IDeliveryRepository _deliveryRepo;
    private readonly IRepositorioSucursal _sucursalRepo; // ⬅️ AGREGAR ESTO

    public AuthService(
        IRepositorioUsuario usuarioRepo,
        IDeliveryRepository deliveryRepo,
        IRepositorioSucursal sucursalRepo) // ⬅️ AGREGAR ESTO
    {
        _usuarioRepo = usuarioRepo;
        _deliveryRepo = deliveryRepo;
        _sucursalRepo = sucursalRepo; // ⬅️ AGREGAR ESTO
    }

    public async Task<LoginResponseDto> LoginAsync(string correo, string contrasena)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(correo);

        if (usuario == null)
            throw new Exception("El correo ingresado no está registrado.");

        if (usuario.Contrasena != contrasena)
            throw new Exception("La contraseña es incorrecta.");

        // SOLO aplicar esta validación si NO es delivery
        if (usuario.Rol != RolUsuario.Delivery && !usuario.Activo)
            throw new Exception("Tu cuenta está desactivada. Contacta al administrador.");

        // VALIDAR DELIVERY
        if (usuario.Rol == RolUsuario.Delivery)
        {
            var delivery = await _deliveryRepo.GetByUsuarioIdAsync(usuario.IdUsuario);

            if (delivery == null)
                throw new Exception("No existe un perfil de delivery asociado a este usuario.");

            if (!delivery.Activo)
                throw new Exception("Tu cuenta de delivery está desactivada. Contacta al administrador.");
        }

        // ⬇️ NUEVO: Obtener las sucursales del usuario
        var sucursales = await _sucursalRepo.ObtenerPorUsuarioAsync(usuario.IdUsuario);

        // ⬇️ NUEVO: Construir respuesta con sucursales
        var response = new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol.ToString(),
            Sucursales = sucursales
                .Where(s => s.Activa) // Solo sucursales activas
                .Select(s => new SucursalLoginDto
                {
                    IdSucursal = s.IdSucursal,
                    NombreSucursal = s.NombreSucursal,
                    Activa = s.Activa
                })
                .ToList()
        };

        return response;
    }
}