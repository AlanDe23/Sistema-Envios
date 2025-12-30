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
                .Include(d => d.Sucursal)
                .ToListAsync();
        }

        public async Task Activar(int id)
        {
            try
            {
                var Delievery = await _context.Delivery
                    .FirstOrDefaultAsync(D => D.IdDelivery == id);

                if (Delievery == null)
                    throw new Exception("Delievery no encontrado");

                Delievery.Activo = true;

                _context.Delivery.Update(Delievery);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al activar el Delievery: {ex.Message}");
            }
        }




        public async Task Desactivar(int id)
        {
            try
            {
                var Delivery = await _context.Delivery
                    .FirstOrDefaultAsync(D => D.IdDelivery == id);

                if (Delivery == null)
                    throw new Exception("Delivery no encontrado");

                Delivery.Activo = false;

                _context.Delivery.Update(Delivery);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al desactivar el Delievery: {ex.Message}");
            }
        }

        public Task<Delivery> GetByEmailAsync(string correo)
        {
            throw new NotImplementedException();
        }



        public async Task<Delivery?> GetByUsuarioIdAsync(int idUsuario)
        {
            return await _context.Delivery
                .Include(d => d.Sucursal)
                .FirstOrDefaultAsync(d => d.IdUsuario == idUsuario);
        }




        public async Task<IEnumerable<Delivery>> GetBySucursalAsync(int idSucursal)
        {
            return await _context.Delivery
                .Include(d => d.Usuario)
                .Where(x => x.IdSucursal == idSucursal && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Delivery> GetByIdAndSucursalAsync(int idDelivery, int idSucursal)
        {
            return await _context.Delivery
                .Include(d => d.Usuario)
                .Where(x => x.IdDelivery == idDelivery &&
                            x.IdSucursal == idSucursal &&
                            !x.IsDeleted)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Delivery>> GetAllBySucursalAsync(int idSucursal)
        {
            return await _context.Delivery
                .Include(d => d.Usuario)
                .Where(d => d.IdSucursal == idSucursal)
                .ToListAsync();
        }



    }
}
