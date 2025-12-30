using Envios.Application.Service;
using Envios.Application.Service.Envios.Application.Services;
using Envios.Application.Service.Interface;
using Envios.Application.Services;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Envios.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ================================
            // Controllers + JSON
            // ================================
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("UsuarioHeader", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "X-Usuario-Id",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Id del usuario (modo desarrollo)"
                });

                options.AddSecurityDefinition("SucursalHeader", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "X-Sucursal-Id",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Id de la sucursal activa"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "UsuarioHeader"
                }
            },
            Array.Empty<string>()
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "SucursalHeader"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            // ================================
            // DbContext
            // ================================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));

            // ================================
            // Repositorios
            // ================================
            builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
            builder.Services.AddScoped<IRepositorioBalancePagado, RepositorioBalancePagado>();
            builder.Services.AddScoped<IRepositorioSuscripcion, RepositorioSuscripcion>();
            builder.Services.AddScoped<IRepositorioSucursal, RepositorioSucursal>();
            builder.Services.AddScoped<IRepositorioBalanceAdmin, RepositorioBalance>();

            // ================================
            // Servicios
            // ================================
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IBalanceService, BalanceService>();
            builder.Services.AddScoped<ISucursalService, SucursalService>();
            builder.Services.AddScoped<SuscripcionService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<PedidoServiceDelivery>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UsuarioService>();

            // ================================
            // CORS
            // ================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "http://127.0.0.1:5500",
                            "http://localhost:5500",
                            "http://127.0.0.1:5501",
                            "http://localhost:5501")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            // ================================
            // Middleware
            // ================================
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            // 🏢 Middleware de sucursal
            app.UseMiddleware<SucursalMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
