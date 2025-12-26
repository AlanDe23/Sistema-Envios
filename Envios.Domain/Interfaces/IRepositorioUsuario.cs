using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{
    public interface IRepositorioUsuario: IRepositorioGenerico<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string correo);

        Task Activar(int id);
        Task Desactivar(int id);
        Task<Usuario?> GetByTokenAsync(string token);
    }
}
