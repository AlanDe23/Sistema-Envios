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
            // Controllers + JSON (Enums como string)
            // ================================
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ================================
            // DbContext
            // ================================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));

            // ================================
            // Repositorios (Genérico + Específicos)
            // ================================
            builder.Services.AddScoped(typeof(IRepositorioGenerico<>), typeof(RepositorioGenerico<>));
            builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
            builder.Services.AddScoped<IRepositorioBalancePagado, RepositorioBalancePagado>();


            builder.Services.AddScoped<IRepositorioBalanceAdmin, RepositorioBalance>();
            builder.Services.AddScoped<IBalanceService, BalanceService>();

            // ================================
            // Servicios de Aplicación
            // ================================
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IBalanceService, BalanceService>(); 


            // **Registro de PedidoServiceDelivery**
            builder.Services.AddScoped<PedidoServiceDelivery>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UsuarioService>();
            // ================================
            // Habilitar CORS (✅ antes de Build)
            // ================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "http://127.0.0.1:5501", "http://localhost:5501")
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

            app.MapControllers();

            app.Run();
        }
    }
}
