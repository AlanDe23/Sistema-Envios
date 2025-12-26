
using Envios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Infrastructure.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<BalanceAdmin> BalanceAdmin { get; set; }
        public DbSet<Delivery> Delivery { get; set; }

        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<BalancePagado> BalancePagado { get; set; }

        public DbSet<Suscripcion> Suscripciones { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }

        public DbSet<UsuarioSucursales> UsuarioSucursales { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 BalanceAdmin ↔ Delivery (1 a 1)
            modelBuilder.Entity<BalanceAdmin>()
                .HasOne(b => b.Delivery)
                .WithOne(d => d.Balance)
                .HasForeignKey<BalanceAdmin>(b => b.IdDelivery)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Pedido ↔ Sucursal (MUY IMPORTANTE)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Sucursal)
                .WithMany()
                .HasForeignKey(p => p.IdSucursal)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Delivery ↔ Sucursal
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Sucursal)
                .WithMany()
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 BalanceAdmin ↔ Sucursal
            modelBuilder.Entity<BalanceAdmin>()
                .HasOne(b => b.Sucursal)
                .WithMany()
                .HasForeignKey(b => b.IdSucursal)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 BalancePagado ↔ Sucursal
            modelBuilder.Entity<BalancePagado>()
                .HasOne(b => b.Sucursal)
                .WithMany()
                .HasForeignKey(b => b.IdSucursal)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Filtro global para pedidos no eliminados
            modelBuilder.Entity<Pedido>()
                .HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.Entity<UsuarioSucursales>()
            .HasKey(us => new { us.IdUsuario, us.IdSucursal });

            modelBuilder.Entity<UsuarioSucursales>()
                .HasOne(us => us.Usuario)
                .WithMany(u => u.UsuarioSucursales)
                .HasForeignKey(us => us.IdUsuario);

            modelBuilder.Entity<UsuarioSucursales>()
                .HasOne(us => us.Sucursal)
                .WithMany(s => s.UsuarioSucursales)
                .HasForeignKey(us => us.IdSucursal);




        }

    }
}
