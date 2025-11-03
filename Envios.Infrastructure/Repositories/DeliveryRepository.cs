using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class DeliveryRepository : ReposGenery.RepositorioGenerico<Delivery>, IDeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Delivery> GetByIdAsync(int id)
        {
            return await _context.Delivery
                .Include(d => d.Usuario)  // 👈 Incluye el Usuario relacionado
                .FirstOrDefaultAsync(d => d.IdDelivery == id);
        }

        public async Task<Delivery> GetByUserIdAsync(int idUsuario)
        {
            return await _context.Delivery.FirstOrDefaultAsync(d => d.IdUsuario == idUsuario);
        }


        public async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _context.Delivery
                .Include(d => d.Usuario)  // 👈 carga el usuario relacionado
                .ToListAsync();
        }
    }
}
