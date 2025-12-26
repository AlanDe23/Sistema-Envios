using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioBalance : RepositorioGenerico<BalanceAdmin>, IRepositorioBalanceAdmin
    {
        private readonly AppDbContext _context;

        public RepositorioBalance(AppDbContext context) : base(context)
        {
            _context = context;
        }




        public async Task<BalanceAdmin> GetBalanceActualAsync()
        {
            return await _context.BalanceAdmin
                 .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.FechaActualizacion)
                .FirstOrDefaultAsync();
        }

        public async Task ResetBalanceAsync()
        {
            var balances = await _context.BalanceAdmin.ToListAsync();
            foreach (var balance in balances)
            {
                balance.TotalEntregados = 0;
                balance.TotalMontoPedidos = 0;
                balance.Pagado = false;
            }
            await _context.SaveChangesAsync();
        }





        public async Task AgregarAsync(BalanceAdmin balance)
        {
            await _context.BalanceAdmin.AddAsync(balance);
            await _context.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(int id)
        {
            var balance = await _context.BalanceAdmin.FindAsync(id);
            balance.IsDeleted = true;
            await _context.SaveChangesAsync();
        }


        public async Task<BalanceAdmin?> GetByIdAsync(int idBalance)
        {
            return await _context.BalanceAdmin
                .AsTracking()
                .FirstOrDefaultAsync(b => b.IdBalance == idBalance && !b.IsDeleted);
        }



        public async Task<int> GuardarCambiosAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task<List<BalanceAdmin>> GetBalancesBySucursalAsync(int idSucursal)
        {
            return await _context.BalanceAdmin
                .Where(b => b.IdSucursal == idSucursal && !b.IsDeleted)
                .ToListAsync();
        }


        public async Task<BalanceAdmin> GetByIdAndSucursalAsync(int idBalance, int idSucursal)
        {
            return await _context.BalanceAdmin
                .Where(x => x.IdBalance == idBalance &&
                            x.IdSucursal == idSucursal &&
                            !x.IsDeleted)
                .FirstOrDefaultAsync();
        }



        public async Task<BalanceAdmin?> GetByDeliveryAndSucursalAsync(int idDelivery, int idSucursal)
        {
            return await _context.BalanceAdmin
                .AsTracking()
                .FirstOrDefaultAsync(b =>
                    b.IdDelivery == idDelivery &&
                    b.IdSucursal == idSucursal &&
                    !b.IsDeleted);
        }

        public async Task SoftDeleteAsync(int idBalance, int idSucursal)
        {
            var balance = await _context.BalanceAdmin
                .FirstOrDefaultAsync(b =>
                    b.IdBalance == idBalance &&
                    b.IdSucursal == idSucursal &&
                    !b.IsDeleted
                );

            if (balance == null)
                throw new Exception("Balance no encontrado para esta sucursal");

            balance.IsDeleted = true;
            balance.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
