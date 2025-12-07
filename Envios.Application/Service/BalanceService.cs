using Envios.Application.Service.Interface;
using Envios.Domain.Entities;
using Envios.Domain.Enum;
using Envios.Domain.Interfaces;

public class BalanceService : IBalanceService
{
    private readonly IRepositorioBalanceAdmin _repositorioBalance;
    private readonly IRepositorioBalancePagado _repositorioBalancePagado;
    private readonly IRepositorioPedido _repositorioPedido;

    public BalanceService(IRepositorioBalanceAdmin repositorioBalance,
                          IRepositorioBalancePagado repositorioBalancePagado,
                          IRepositorioPedido repositorioPedido)
    {
        _repositorioBalance = repositorioBalance;
        _repositorioBalancePagado = repositorioBalancePagado;
        _repositorioPedido = repositorioPedido;
    }

    public async Task<bool> MarcarComoPagado(int idBalance)
    {
        var balance = await _repositorioBalance.GetByIdAsync(idBalance);
        if (balance == null) return false;

        var historial = new BalancePagado
        {
            IdBalance = balance.IdBalance,
            IdDelivery = balance.IdDelivery,
            TotalEntregados = (int)balance.TotalEntregados,
            TotalMontoPedidos = balance.TotalMontoPedidos,
            FechaPago = DateTime.Now
        };

        await _repositorioBalancePagado.AgregarAsync(historial);

        balance.Pagado = true;
        balance.TotalEntregados = 0;
        balance.TotalMontoPedidos = 0;

        await _repositorioBalance.ActualizarAsync(balance);

        return true;
    }

    // 🔑 Método público que respeta la interfaz
    public async Task<object> ObtenerBalanceDeliveryAsync(int idDelivery)
    {
        // Por defecto, se comporta como "crearSiNoExiste = true"
        return await ObtenerBalanceDeliveryInterno(idDelivery, true);
    }

    // 👇 Método auxiliar interno con el flag

    public async Task<bool> EliminarBalanceAsync(int idBalance)
    {
        var balance = await _repositorioBalance.GetByIdAsync(idBalance);
        if (balance == null)
            return false;

        int idDelivery = balance.IdDelivery;
        bool estabaPagado = balance.Pagado;

        var pedidos = await _repositorioPedido.GetAllAsync();
        var pedidosAsociados = pedidos.Where(p => p.IdDelivery == idDelivery).ToList();

        // ✅ SOLO marcar como eliminado (no borrar físico)
        foreach (var pedido in pedidosAsociados)
        {
            pedido.IsDeleted = true;
            await _repositorioPedido.ActualizarAsync(pedido);
        }

        // ✅ Soft delete balance
        balance.IsDeleted = true;
        await _repositorioBalance.ActualizarAsync(balance);

        if (!estabaPagado)
        {
            await ActualizarBalanceDeliveryAsync(idDelivery);
        }

        return true;
    }



    // 👇 Método auxiliar interno con el flag
    private async Task<object> ObtenerBalanceDeliveryInterno(int idDelivery, bool crearSiNoExiste)
    {
        var balanceExistente = await _repositorioBalance.GetByDeliveryAsync(idDelivery);

        if (balanceExistente != null)
            return balanceExistente;

        if (!crearSiNoExiste)
            return null;

        var pedidos = await _repositorioPedido.GetAllAsync();
        var entregados = pedidos
            .Where(p => p.IdDelivery == idDelivery && p.Estado == EstadoPedido.Entregado.ToString())
            .ToList();

        if (!entregados.Any())
            return new { mensaje = "No hay pedidos entregados para este delivery" };

        decimal totalEfectivo = entregados
            .Where(p => p.MetodoPago == MetodoPago.Efectivo.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalTransferencias = entregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalEnviosTransferencias = entregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioEnvio);

        decimal totalEfectivoNeto = totalEfectivo - totalEnviosTransferencias;

        decimal totalFinalAdmin = entregados.Sum(p => p.PrecioPedido);


        var balance = new BalanceAdmin
        {
            IdDelivery = idDelivery,
            TotalTransferencias = totalTransferencias,
            TotalEfectivoBruto = totalEfectivo,
            TotalEnviosTransferencias = totalEnviosTransferencias,
            TotalEfectivoNeto = totalEfectivoNeto,
            TotalFinalAdmin = totalFinalAdmin,
            TotalPedidosEntregados = entregados.Count,
            FechaActualizacion = DateTime.Now,
            Pagado = false,
            TotalMontoPedidos = entregados.Sum(p => p.PrecioPedido),
            TotalEntregados = entregados.Count
        };

        await _repositorioBalance.AgregarAsync(balance);

        return balance;
    }


    public async Task ActualizarBalanceDeliveryAsync(int idDelivery)
    {
        var pedidos = await _repositorioPedido.GetAllAsync();
        var pedidosEntregados = pedidos
            .Where(p => p.IdDelivery == idDelivery && p.Estado == EstadoPedido.Entregado.ToString())
            .ToList();

        if (!pedidosEntregados.Any())
            return;

        // 🔍 Buscar balances existentes del delivery
        var balances = await _repositorioBalance.GetAllAsync();
        var balanceExistente = balances.FirstOrDefault(b => b.IdDelivery == idDelivery);
        var balancePagado = balances.FirstOrDefault(b => b.IdDelivery == idDelivery && b.Pagado);

        // 🚫 Si ya hubo un balance pagado, NO volver a crear ni actualizar
        if (balancePagado != null)
            return;

        // 💰 Calcular totales por método de pago
        decimal totalEfectivo = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Efectivo.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalTransferencias = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalEnviosTransferencias = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioEnvio);

        // 💵 Efectivo neto (restando los envíos por transferencia)
        decimal totalEfectivoNeto = totalEfectivo - totalEnviosTransferencias;

        // 🧾 TotalFinalAdmin = suma de todos los pedidos (efectivo + transferencias)
        decimal totalFinalAdmin = pedidosEntregados.Sum(p => p.PrecioPedido);

        // ⚙️ Si ya existe un balance pendiente → actualizamos
        if (balanceExistente != null && !balanceExistente.Pagado)
        {
            balanceExistente.TotalTransferencias = totalTransferencias;
            balanceExistente.TotalEfectivoBruto = totalEfectivo;
            balanceExistente.TotalEnviosTransferencias = totalEnviosTransferencias;
            balanceExistente.TotalEfectivoNeto = totalEfectivoNeto;
            balanceExistente.TotalFinalAdmin = totalFinalAdmin;
            balanceExistente.TotalPedidosEntregados = pedidosEntregados.Count;
            balanceExistente.FechaActualizacion = DateTime.UtcNow;
            balanceExistente.TotalMontoPedidos = totalFinalAdmin;
            balanceExistente.TotalEntregados = pedidosEntregados.Count;

            await _repositorioBalance.ActualizarAsync(balanceExistente);
        }
        // 🆕 Si no existe ningún balance → lo creamos
        else if (balanceExistente == null)
        {
            var nuevoBalance = new BalanceAdmin
            {
                IdDelivery = idDelivery,
                TotalTransferencias = totalTransferencias,
                TotalEfectivoBruto = totalEfectivo,
                TotalEnviosTransferencias = totalEnviosTransferencias,
                TotalEfectivoNeto = totalEfectivoNeto,
                TotalFinalAdmin = totalFinalAdmin,
                TotalPedidosEntregados = pedidosEntregados.Count,
                FechaActualizacion = DateTime.UtcNow,
                Pagado = false,
                TotalMontoPedidos = totalFinalAdmin,
                TotalEntregados = pedidosEntregados.Count
            };

            await _repositorioBalance.AgregarAsync(nuevoBalance);
        }
    }



}
