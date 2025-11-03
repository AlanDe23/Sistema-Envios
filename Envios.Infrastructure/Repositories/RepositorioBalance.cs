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

   


        public async Task<BalanceAdmin> GetUltimoBalancePorAdmin(int idAdmin)
        {
            return await _context.BalanceAdmin
                .Where(b => b.IdBalance == idAdmin)
                .OrderByDescending(b => b.FechaActualizacion)
                .FirstOrDefaultAsync();
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


        public async Task<BalanceAdmin> GetByDeliveryAsync(int idDelivery)
        {
            return await _context.BalanceAdmin
                .Where(b => b.IdDelivery == idDelivery && !b.IsDeleted)
                .FirstOrDefaultAsync();
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




    }
}
