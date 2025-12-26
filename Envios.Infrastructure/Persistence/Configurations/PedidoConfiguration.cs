using Envios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Infrastructure.Persistence.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(p => p.IdPedido);

            builder.Property(p => p.Descripcion)
                   .HasMaxLength(255);

            builder.Property(p => p.PrecioPedido)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PrecioEnvio)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.MetodoPago)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(p => p.Estado)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

        }
    }

}
