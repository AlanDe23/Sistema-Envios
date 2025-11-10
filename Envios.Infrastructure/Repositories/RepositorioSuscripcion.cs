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

        public async Task CrearAsync(Suscripcion suscripcion)
        {
            await _context.Suscripciones.AddAsync(suscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task<Suscripcion?> ObtenerPorIdAsync(int id)
        {
            return await _context.Suscripciones.FindAsync(id);
        }

        public async Task<IEnumerable<Suscripcion>> ObtenerPorUsuarioAsync(int  usuarioId)
        {
            return await _context.Suscripciones.Where(s => s.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task ActualizarAsync(Suscripcion suscripcion)
        {
            _context.Suscripciones.Update(suscripcion);
            await _context.SaveChangesAsync();
        }
    }
}
