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
                if (id <= 0)
                    throw new ArgumentException("El ID proporcionado no es válido.");

                var keyName = _context.Model.FindEntityType(typeof(T))
                    ?.FindPrimaryKey()
                    ?.Properties.First().Name;

                if (keyName == null)
                    throw new Exception($"No se encontró la llave primaria de {typeof(T).Name}");

                var entity = await _context.Set<T>()
                    .FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) == id);

                if (entity == null)
                    throw new KeyNotFoundException($"No existe un {typeof(T).Name} con ID {id}");

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en GetByIdAsync para {typeof(T).Name}", ex);
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
                _context.Entry(entidad).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar {typeof(T).Name}", ex);
            }
        }


        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
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
