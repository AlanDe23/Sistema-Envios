using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Envios.Infrastructure.Repositories.ReposGenery
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public RepositorioGenerico(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener {typeof(T).Name} por Id: {id}", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener todos los registros de {typeof(T).Name}", ex);
            }
        }

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado)
        {
            try
            {
                return await _dbSet.Where(predicado).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar {typeof(T).Name}", ex);
            }
        }

        public async Task AgregarAsync(T entidad)
        {
            try
            {
                await _dbSet.AddAsync(entidad); 
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar {typeof(T).Name}", ex);
            }
        }

        public async Task ActualizarAsync(T entidad)
        {
            try
            {
                _dbSet.Update(entidad);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar {typeof(T).Name}", ex);
            }
        }

        public async Task EliminarAsync(T entidad)
        {
            try
            {
                _dbSet.Remove(entidad);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar {typeof(T).Name}", ex);
            }
        }
    }
}
