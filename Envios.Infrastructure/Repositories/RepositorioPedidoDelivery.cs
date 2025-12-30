using Envios.Domain.Entities;
using Envios.Domain.Enum;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioPedidoDelivery : RepositorioGenerico<Pedido>, IRepositorioPedido
    {
        private readonly AppDbContext _context;

        public RepositorioPedidoDelivery(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // 🔹 Obtener pedidos de una fecha específica
        public async Task<IEnumerable<Pedido>> GetPedidosPorFecha(DateTime fecha)
        {
            try
            {
                return await _context.Pedido
                    .Where(p => p.FechaCreacion.Date == fecha.Date && !p.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener pedidos por fecha", ex);
            }
        }

        // 🔹 Eliminar (lógicamente) todos los pedidos de una fecha específica
        public async Task<Pedido> DeletePedidosPorFecha(DateTime fecha)
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Where(p => p.FechaCreacion.Date == fecha.Date && !p.IsDeleted)
                    .ToListAsync();

                if (!pedidos.Any())
                    throw new Exception("No existen pedidos en esa fecha");

                foreach (var pedido in pedidos)
                    pedido.IsDeleted = true;

                await _context.SaveChangesAsync();

                return pedidos.First(); // Devuelvo uno de referencia
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar pedidos por fecha", ex);
            }
        }

        // 🔹 Eliminar (lógicamente) pedidos de un mes específico
        public async Task<Pedido> DeletePedidoPorMes(int year, int mes)
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Where(p => p.FechaCreacion.Year == year && p.FechaCreacion.Month == mes && !p.IsDeleted)
                    .ToListAsync();

                if (!pedidos.Any())
                    throw new Exception("No existen pedidos en ese mes");

                foreach (var pedido in pedidos)
                    pedido.IsDeleted = true;

                await _context.SaveChangesAsync();

                return pedidos.First(); // Devuelvo uno de referencia
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar pedidos por mes", ex);
            }
        }

        // 🔹 Obtener pedidos entregados (sin eliminados)
        public async Task<IEnumerable<Pedido>> GetPedidosEntregadosAsync()
        {
            return await _context.Pedido
                .Where(p => p.Estado == "Entregado" && !p.IsDeleted)
                .ToListAsync();
        }

        // 🔹 Eliminar (lógicamente) por ID
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
                return false;

            pedido.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // 🔹 Agregar múltiples pedidos (sin cambios)
        public async Task AgregarAsync(IEnumerable<Pedido> pedidos)
        {
            if (pedidos == null || !pedidos.Any())
                throw new ArgumentNullException(nameof(pedidos), "La lista de pedidos no puede estar vacía");

            try
            {
                await _context.Pedido.AddRangeAsync(pedidos);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los pedidos", ex);
            }
        }

        // 🔹 Obtener pedidos por delivery (sin eliminados)
        public async Task<IEnumerable<Pedido>> GetPedidosPorDeliveryAsync(int idDelivery)
        {
            try
            {
                return await _context.Pedido
                    .Where(p => p.IdDelivery == idDelivery && !p.IsDeleted)
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



        public async Task<List<Pedido>> GetEntregadosByDeliveryAsync(int idDelivery, int idSucursal)
        {
            return await _context.Pedido
                .Where(p =>
                    p.IdSucursal == idSucursal &&
                    p.IdDelivery == idDelivery &&
                    p.Estado == EstadoPedido.Entregado.ToString() &&
                    !p.IsDeleted
                )
                .ToListAsync();
        }

    }
}