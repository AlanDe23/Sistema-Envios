using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioUsuario : RepositorioGenerico<Usuario>, IRepositorioUsuario
    {
        private readonly AppDbContext _context;

        public RepositorioUsuario(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Usuario con Id: {id}", ex);
            }
        }

        public async Task<Usuario?> GetByEmailAsync(string correo)
        {
            try
            {
                return await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == correo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Usuario por correo: {correo}", ex);
            }
        }
    }
}
