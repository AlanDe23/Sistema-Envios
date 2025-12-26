using Envios.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class SucursalMiddleware
{
    private readonly RequestDelegate _next;

    public SucursalMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var path = context.Request.Path.Value?.ToLower();

        // 🔹 ENDPOINTS PÚBLICOS (NO VALIDAR SUCURSAL)
        if (
       // 🔐 AUTH
       path.StartsWith("/api/auth/login") ||

       // 👤 USUARIO
       path.StartsWith("/api/usuario/create") ||
       path.StartsWith("/api/usuario/recuperar") ||
       path.StartsWith("/api/usuario/restablecer") ||
       path.StartsWith("/api/usuario/cambiar-contrasena") ||

       // 👤 USUARIO CRUD
       path.StartsWith("/api/usuario/getall") ||
       path.StartsWith("/api/usuario/") ||   // cubre /api/usuario/5
       path.StartsWith("/api/usuario/delete") ||
       path.StartsWith("/api/usuario/activar") ||
       path.StartsWith("/api/usuario/desactivar") ||

       // 🏢 SUCURSAL
       path.StartsWith("/api/sucursal/crear") ||
       path.StartsWith("/api/sucursal/actualizar") ||
       path.StartsWith("/api/sucursal/desactivar") ||
       path.StartsWith("/api/sucursal/activar") ||
       path.StartsWith("/api/sucursal/") ||

       // 📦 SUSCRIPCIONES
       path.StartsWith("/api/suscripciones") ||

       // ✅ VALIDAR TOKEN
       path.StartsWith("/api/validar/validar-token") ||

       // 📧 CONFIRMAR
       path.Contains("/confirmar")
   )
        {
            await _next(context);
            return;
        }


        // 🔹 HEADER USUARIO
        if (!context.Request.Headers.TryGetValue("X-Usuario-Id", out var usuarioHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("X-Usuario-Id no enviado");
            return;
        }

        // 🔹 HEADER SUCURSAL
        if (!context.Request.Headers.TryGetValue("X-Sucursal-Id", out var sucursalHeader))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("X-Sucursal-Id no enviado");
            return;
        }

        if (!int.TryParse(usuarioHeader, out int idUsuario) ||
            !int.TryParse(sucursalHeader, out int idSucursal))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Headers inválidos");
            return;
        }

        // 🔹 VALIDAR ACCESO USUARIO ↔ SUCURSAL
        bool tieneAcceso = await db.UsuarioSucursales.AnyAsync(us =>
            us.IdUsuario == idUsuario &&
            us.IdSucursal == idSucursal
        );

        if (!tieneAcceso)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("No tiene acceso a esta sucursal");
            return;
        }

        // 🔹 GUARDAR EN CONTEXTO
        context.Items["IdUsuario"] = idUsuario;
        context.Items["IdSucursal"] = idSucursal;

        await _next(context);
    }
}
