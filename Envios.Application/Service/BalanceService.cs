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

    public async Task<bool> MarcarComoPagado(int idBalance, int idSucursal)
    {
        var balance = await _repositorioBalance.GetByIdAndSucursalAsync(idBalance, idSucursal);
        if (balance == null) return false;

        var historial = new BalancePagado
        {
            IdBalance = balance.IdBalance,
            IdDelivery = balance.IdDelivery,
            TotalEntregados = (int)balance.TotalEntregados,
            TotalMontoPedidos = balance.TotalMontoPedidos,
            IdSucursal = idSucursal,
            FechaPago =  DateTime.Now
        };

        await _repositorioBalancePagado.AgregarAsync(historial);

        balance.Pagado = true;
        balance.TotalEntregados = 0;
        balance.TotalMontoPedidos = 0;

        await _repositorioBalance.ActualizarAsync(balance);

        return true;
    }

    // 🔑 Método público que respeta la interfaz
    public async Task<object> ObtenerBalanceDeliveryAsync(int idDelivery , int idSucursal)
    {
        // Por defecto, se comporta como "crearSiNoExiste = true"
        return await ObtenerBalanceDeliveryInterno(idDelivery, true , idSucursal);
    }

    // 👇 Método auxiliar interno con el flag

    public async Task<bool> EliminarBalanceAsync(int idBalance , int idSucursal)
    {
        var balance = await _repositorioBalance.GetByIdAndSucursalAsync(idBalance , idSucursal);
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
            await ActualizarBalanceDeliveryAsync(idDelivery , idSucursal);
        }

        return true;
    }



    // 👇 Método auxiliar interno con el flag
    private async Task<BalanceAdmin> ObtenerBalanceDeliveryInterno(int idDelivery, bool crearSiNoExiste , int idSucursal)
    {
        var balanceExistente = await _repositorioBalance.GetByDeliveryAndSucursalAsync(idDelivery , idSucursal);

        if (balanceExistente != null)
            return balanceExistente;

        if (!crearSiNoExiste)
            return null;

        var pedidos = await _repositorioPedido.GetAllBySucursalAsync(idSucursal);
        var entregados = pedidos
            .Where(p => p.IdDelivery == idDelivery && p.Estado == EstadoPedido.Entregado.ToString())
            .ToList();

        if (!entregados.Any())
            return null; 

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
            IdSucursal = idSucursal,
            Pagado = false,
            TotalMontoPedidos = entregados.Sum(p => p.PrecioPedido),
            TotalEntregados = entregados.Count
        };

        await _repositorioBalance.AgregarAsync(balance);

        return balance;
    }

    public async Task ActualizarBalanceDeliveryAsync(int idDelivery , int idSucursal)
    {
        var pedidos = await _repositorioPedido.GetEntregadosByDeliveryAsync(idDelivery, idSucursal);
        var pedidosEntregados = pedidos
            .Where(p => p.IdDelivery == idDelivery && p.Estado == EstadoPedido.Entregado.ToString())
            .ToList();


        foreach (var p in pedidosEntregados)
        {
            Console.WriteLine(
                $"Pedido {p.IdPedido} | Estado={p.Estado} | MetodoPago='{p.MetodoPago}' | PrecioPedido={p.PrecioPedido} | PrecioEnvio={p.PrecioEnvio}"
            );
        }

        if (!pedidosEntregados.Any())
            return;

        // 1) Obtener sólo EL balance para este delivery (tracked)
        var balanceExistente = await _repositorioBalance.GetByDeliveryAndSucursalAsync(idDelivery , idSucursal);

        // 2) Comprobación de si existe balance pagado (también la hacemos por repo)
        var balancePagado = balanceExistente != null && balanceExistente.Pagado;
        if (balancePagado)
            return;

        // 3) Calculos
        decimal totalEfectivo = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Efectivo.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalTransferencias = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioPedido);

        decimal totalEnviosTransferencias = pedidosEntregados
            .Where(p => p.MetodoPago == MetodoPago.Transferencia.ToString())
            .Sum(p => p.PrecioEnvio);

        decimal totalEfectivoNeto = totalEfectivo - totalEnviosTransferencias;
        decimal totalFinalAdmin = pedidosEntregados.Sum(p => p.PrecioPedido);

        // 4) Si existe y NO está pagado -> modificar la instancia trackeada y guardar
        if (balanceExistente != null)
        {
            balanceExistente.TotalTransferencias = totalTransferencias;
            balanceExistente.TotalEfectivoBruto = totalEfectivo;
            balanceExistente.TotalEnviosTransferencias = totalEnviosTransferencias;
            balanceExistente.TotalEfectivoNeto = totalEfectivoNeto;
            balanceExistente.TotalFinalAdmin = totalFinalAdmin;
            balanceExistente.TotalPedidosEntregados = pedidosEntregados.Count;
            balanceExistente.FechaActualizacion = DateTime.Now;
            balanceExistente.TotalMontoPedidos = totalFinalAdmin;
            balanceExistente.TotalEntregados = pedidosEntregados.Count;

            // Guardar cambios sobre la instancia TRACKED
            await _repositorioBalance.GuardarCambiosAsync();
            Console.WriteLine($"Balance actualizado para delivery {idDelivery}");
            return;
        }

        // 5) Si no existe balance -> crear
        var nuevoBalance = new BalanceAdmin
        {
            IdDelivery = idDelivery,
            TotalTransferencias = totalTransferencias,
            TotalEfectivoBruto = totalEfectivo,
            TotalEnviosTransferencias = totalEnviosTransferencias,
            TotalEfectivoNeto = totalEfectivoNeto,
            TotalFinalAdmin = totalFinalAdmin,
            TotalPedidosEntregados = pedidosEntregados.Count,
            FechaActualizacion = DateTime.Now,
            Pagado = false,
            IdSucursal = idSucursal,
            TotalMontoPedidos = totalFinalAdmin,
            TotalEntregados = pedidosEntregados.Count
        };

        await _repositorioBalance.AgregarAsync(nuevoBalance);
        Console.WriteLine($"Nuevo balance creado para delivery {idDelivery}");
    }




}
