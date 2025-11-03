using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using Envios.Infrastructure.Persistence.Data;
using Envios.Infrastructure.Repositories.ReposGenery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Infrastructure.Repositories
{
    public class RepositorioBalancePagado:RepositorioGenerico<BalancePagado>, IRepositorioBalancePagado
    {

        private readonly AppDbContext _context;

        public RepositorioBalancePagado(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
