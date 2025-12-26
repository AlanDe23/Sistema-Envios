using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioPedido : RepositorioGenerico<Pedido>, IRepositorioPedido
    {
        private readonly AppDbContext _context;

        public RepositorioPedido(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetPedidosPorFecha(DateTime fecha)
        {
            return await _context.Pedido
                .Where(p => p.Estado != null && p.Estado != "" && EF.Property<DateTime>(p, "FechaCreacion").Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<Pedido> DeletePedidosPorFecha(DateTime fecha)
        {
            var pedido = await _context.Pedido
                .FirstOrDefaultAsync(p => EF.Property<DateTime>(p, "FechaCreacion").Date == fecha.Date);

            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return pedido;
        }

        public async Task<Pedido> DeletePedidoPorMes(int year, int mes)
        {
            var pedido = await _context.Pedido
                .FirstOrDefaultAsync(p => EF.Property<DateTime>(p, "FechaCreacion").Year == year &&
                                          EF.Property<DateTime>(p, "FechaCreacion").Month == mes);

            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return pedido;
        }
        public async Task<IEnumerable<Pedido>> GetPedidosEntregadosAsync()
        {
            return await _context.Pedido
                .Where(p => p.Estado == "Entregado")
                .ToListAsync();
        }


        public async Task<bool> DeleteByIdAsync(int id)
        {
            var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return false;

            pedido.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedido
                .Include(p => p.Delivery)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task AgregarAsync(IEnumerable<Pedido> pedidos)
        {
            if (pedidos == null)
                throw new ArgumentNullException(nameof(pedidos));

            await _context.Pedido.AddRangeAsync(pedidos); // Agrega todos los pedidos
            await _context.SaveChangesAsync();           // Guarda los cambios en la base de datos
        }



        public async Task<IEnumerable<Pedido>> GetPedidosPorDeliveryAsync(int idDelivery)
        {
            try
            {
                return await _context.Pedido
                    .Where(p => p.IdDelivery == idDelivery)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener pedidos del delivery", ex);
            }
        }

        public async Task<IEnumerable<Pedido>> GetAllBySucursalAsync(int idSucursal)
        {
            return await _context.Pedido
                .Include(p => p.Delivery)
                .Where(x => x.IdSucursal == idSucursal && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Pedido> GetByIdAndSucursalAsync(int idPedido, int idSucursal)
        {
            return await _context.Pedido
                .Where(x => x.IdPedido == idPedido &&
                            x.IdSucursal == idSucursal &&
                            !x.IsDeleted)
                .FirstOrDefaultAsync();
        }


    }
}
