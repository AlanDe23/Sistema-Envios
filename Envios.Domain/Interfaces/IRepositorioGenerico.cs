using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado);
        Task AgregarAsync(T entidad);
        Task ActualizarAsync(T entidad);
        Task EliminarAsync(T entidad);
    }
}
