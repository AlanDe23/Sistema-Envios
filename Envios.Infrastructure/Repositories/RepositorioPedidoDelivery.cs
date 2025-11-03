using Envios.Domain.Entities;
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
                    .Where(p => p.FechaCreacion.Date == fecha.Date)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener pedidos por fecha", ex);
            }
        }

        // 🔹 Eliminar todos los pedidos de una fecha específica
        public async Task<Pedido> DeletePedidosPorFecha(DateTime fecha)
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Where(p => p.FechaCreacion.Date == fecha.Date)
                    .ToListAsync();

                if (!pedidos.Any())
                    throw new Exception("No existen pedidos en esa fecha");

                _context.Pedido.RemoveRange(pedidos);
                await _context.SaveChangesAsync();

                return pedidos.First(); // Devuelvo uno de referencia
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar pedidos por fecha", ex);
            }
        }

        // 🔹 Eliminar pedidos de un mes específico
        public async Task<Pedido> DeletePedidoPorMes(int year, int mes)
        {
            try
            {
                var pedidos = await _context.Pedido
                    .Where(p => p.FechaCreacion.Year == year && p.FechaCreacion.Month == mes)
                    .ToListAsync();

                if (!pedidos.Any())
                    throw new Exception("No existen pedidos en ese mes");

                _context.Pedido.RemoveRange(pedidos);
                await _context.SaveChangesAsync();

                return pedidos.First(); // Devuelvo uno de referencia
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar pedidos por mes", ex);
            }
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

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task AgregarAsync(IEnumerable<Pedido> pedidos)
        {
            if (pedidos == null || !pedidos.Any())
                throw new ArgumentNullException(nameof(pedidos), "La lista de pedidos no puede estar vacía");

            try
            {
                await _context.Pedido.AddRangeAsync(pedidos); // Agrega todos los pedidos
                await _context.SaveChangesAsync();           // Guarda los cambios en la base de datos
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar los pedidos", ex);
            }
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


    }
}
