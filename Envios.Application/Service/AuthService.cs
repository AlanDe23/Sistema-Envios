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

        if (usuario.Rol != RolUsuario.Delivery && !usuario.Activo)
            throw new Exception("Tu cuenta está desactivada.");

        var response = new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = usuario.Rol.ToString(),
            Sucursales = new List<SucursalLoginDto>()
        };

        // 🔹 CASO DELIVERY
        if (usuario.Rol == RolUsuario.Delivery)
        {
            var delivery = await _deliveryRepo.GetByUsuarioIdAsync(usuario.IdUsuario);

            if (delivery == null)
                throw new Exception("Delivery sin sucursal asignada.");

            if (!delivery.Activo)
                throw new Exception("Tu cuenta de delivery está desactivada.");

            // Asegurarse de que la sucursal esté cargada
            var sucursal = delivery.Sucursal;
            if (sucursal == null)
            {
                // Si la sucursal no está cargada, obtenerla desde el repositorio de sucursales
                sucursal = (await _sucursalRepo.ObtenerPorUsuarioAsync(usuario.IdUsuario))
                    .FirstOrDefault(s => s.IdSucursal == delivery.IdSucursal);
            }

            response.Sucursales.Add(new SucursalLoginDto
            {
                IdSucursal = delivery.IdSucursal,
                NombreSucursal = sucursal?.NombreSucursal ?? string.Empty

            });

            return response; // ⬅️ FIN AQUÍ
        }

        // 🔹 CASO ADMIN / OTROS ROLES
        var sucursales = await _sucursalRepo.ObtenerPorUsuarioAsync(usuario.IdUsuario);

        response.Sucursales = sucursales
            .Where(s => s.Activa)
            .Select(s => new SucursalLoginDto
            {
                IdSucursal = s.IdSucursal,
                NombreSucural = s.NombreSucursal
            
            })
            .ToList();

        return response;
    }
}


