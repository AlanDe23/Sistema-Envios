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
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Delivery");

            builder.HasKey(d => d.IdDelivery);

            builder.Property(d => d.Telefono)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(d => d.BalanceAcumulado)
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.Estado)
                .IsRequired();


        }
    }
}
