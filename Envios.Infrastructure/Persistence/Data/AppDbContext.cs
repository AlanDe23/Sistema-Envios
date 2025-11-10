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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Configuración de relación BalanceAdmin ↔ Delivery
            modelBuilder.Entity<BalanceAdmin>()
                .HasOne(b => b.Delivery)
                .WithOne(d => d.Balance)
                .HasForeignKey<BalanceAdmin>(b => b.IdDelivery)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Filtro global para pedidos no eliminados
            modelBuilder.Entity<Pedido>()
                .HasQueryFilter(p => !p.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }



    }

}
