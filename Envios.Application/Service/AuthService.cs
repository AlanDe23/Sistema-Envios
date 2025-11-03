using Envios.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Envios.Domain.Entities;

public class AuthService
{
    private readonly IRepositorioUsuario _usuarioRepo;

    public AuthService(IRepositorioUsuario usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }
    public async Task<Usuario> LoginAsync(string correo, string contrasena)
    {
        try
        {
            var usuario = await _usuarioRepo.GetByEmailAsync(correo);

            if (usuario == null)
                throw new Exception("El correo ingresado no está registrado.");

            if (usuario.Contrasena != contrasena)
                throw new Exception("La contraseña es incorrecta.");

            // Si todo está bien  retorna el usuario
            return usuario;
        }
        catch (Exception ex)
        {
            // Aquí puedes registrar el error en logs o base de datos si quieres
            Console.WriteLine($"[ERROR LOGIN]: {ex.Message}");
            throw; // Re-lanza la excepción para manejarla en la capa superior (controlador, por ejemplo)
        }
    }
}