using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioSuscripcion : IRepositorioSuscripcion
    {
        private readonly AppDbContext _context;

        public RepositorioSuscripcion(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Suscripcion?> GetByCarnetIdAsync(string carnetSubscriptionId)
        {
            return await _context.Suscripciones
                .FirstOrDefaultAsync(x => x.TransaccionId == carnetSubscriptionId);
        }

        public async Task<Suscripcion?> GetActivaByUsuarioAsync(int usuarioId)
        {
            return await _context.Suscripciones
                .Where(x => x.UsuarioId == usuarioId &&
                            x.Estado == "Activa" &&
                            x.FechaFin > DateTime.Now)
                .OrderByDescending(x => x.FechaFin)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Suscripcion suscripcion)
        {
            await _context.Suscripciones.AddAsync(suscripcion);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

