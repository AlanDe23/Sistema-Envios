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
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.IdUsuario);

            builder.Property(u => u.Nombre)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(u => u.Correo)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(u => u.Contrasena)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(u => u.Rol)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(u => u.FechaRegistro)
                   .IsRequired();
        }

    }

}
