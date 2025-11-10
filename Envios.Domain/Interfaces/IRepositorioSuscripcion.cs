using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Domain.Interfaces
{

    public interface IRepositorioSuscripcion
    {
        Task<Suscripcion?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Suscripcion>> ObtenerPorUsuarioAsync(int  usuarioId);
        Task CrearAsync(Suscripcion suscripcion);
        Task ActualizarAsync(Suscripcion suscripcion);
    }

}
