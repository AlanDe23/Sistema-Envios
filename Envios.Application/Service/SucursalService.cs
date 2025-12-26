using Envios.Application.DTOs.SucursalDTO;
using Envios.Application.DTOs.SucursalDTP;
using Envios.Application.Service.Interface;
using Envios.Domain.Entities;
using Envios.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.Service
{
    public class SucursalService : ISucursalService
    {
        private readonly IRepositorioSucursal _repoSucursal;
        private readonly IRepositorioUsuario _repoUsuario;

        public SucursalService(IRepositorioSucursal repoSucursal, IRepositorioUsuario repoUsuario)
        {
            _repoSucursal = repoSucursal;
            _repoUsuario = repoUsuario;
        }

        public async Task CrearSucursalAsync(SucursalCrearDTO dto)
        {
            var usuario = await _repoUsuario.GetByIdAsync(dto.UsuarioId);

            if (usuario == null)
                throw new Exception("El usuario no existe.");

            var sucursal = new Sucursal
            {
                NombreSucursal = dto.NombreSucursal,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Activa = true
            };

            await _repoSucursal.AgregarAsync(sucursal);

            var usuarioSucursal = new UsuarioSucursales
            {
                IdUsuario = dto.UsuarioId,
                IdSucursal = sucursal.IdSucursal
            };


            await _repoSucursal.AgregarUsuarioSucursalAsync(usuarioSucursal);
        }

        public async Task<List<Sucursal>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _repoSucursal.ObtenerPorUsuarioAsync(usuarioId);
        }

        public async Task<Sucursal> ObtenerPorIdAsync(int id)
        {
            return await _repoSucursal.GetByIdAsync(id);
        }

        public async Task ActualizarSucursalAsync(SucursalActualizarDTO dto)
        {
            var sucursal = await _repoSucursal.GetByIdAsync(dto.IdSucursal);

            if (sucursal == null)
                throw new Exception("La sucursal no existe.");

            sucursal.NombreSucursal = dto.NombreSucursal;
            sucursal.Direccion = dto.Direccion;
            sucursal.Telefono = dto.Telefono;

            await _repoSucursal.ActualizarAsync(sucursal);
        }

        public async Task DesactivarSucursalAsync(int id)
        {
            var sucursal = await _repoSucursal.GetByIdAsync(id);

            if (sucursal == null)
                throw new Exception("La sucursal no existe.");

            sucursal.Activa = false;

            await _repoSucursal.ActualizarAsync(sucursal);
        }


        public async Task ActivarSucursalAsync(int id)
        {
            var sucursal = await _repoSucursal.GetByIdAsync(id);

            if (sucursal == null)
                throw new Exception("La sucursal no existe.");

            sucursal.Activa = true;

            await _repoSucursal.ActualizarAsync(sucursal);
        }

    }
}
