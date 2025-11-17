using Envios.Application.DTOs.SucursalDTO;
using Envios.Application.DTOs.SucursalDTP;
using Envios.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.Service.Interface
{
    public interface ISucursalService
    {
        Task CrearSucursalAsync(SucursalCrearDTO dto);
        Task<List<Sucursal>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<Sucursal> ObtenerPorIdAsync(int id);
        Task ActualizarSucursalAsync(SucursalActualizarDTO dto);
        Task DesactivarSucursalAsync(int id);

        Task ActivarSucursalAsync(int id);

    }
}
