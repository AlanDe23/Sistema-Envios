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
        Task<Suscripcion?> GetByCarnetIdAsync(string carnetId);
        Task<Suscripcion?> GetActivaByUsuarioAsync(int usuarioId);
        Task AddAsync(Suscripcion suscripcion);
        Task SaveChangesAsync();
    }

}
