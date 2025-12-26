using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioSucursal : RepositorioGenerico<Sucursal>, IRepositorioSucursal
    {
        private readonly AppDbContext _context;

        public RepositorioSucursal(AppDbContext context) : base(context)
        {
            _context = context;
        }

    



        public async Task AgregarUsuarioSucursalAsync(UsuarioSucursales usuarioSucursal)
        {
            try
            {
                await _context.UsuarioSucursales.AddAsync(usuarioSucursal);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar UsuarioSucursal", ex);
            }
        }


        public async Task<List<Sucursal>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.UsuarioSucursales
                .Where(us => us.IdUsuario == usuarioId)
                .Include(us => us.Sucursal)
                .Select(us => us.Sucursal)
                .AsNoTracking()
                .ToListAsync();
        }



    }
}



