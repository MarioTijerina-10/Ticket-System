using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;
using TicketsApp.DTO.Proveedores;

namespace TicketsApp.Servicio;

public class ServicioProveedores : ServicioBase<ServicioProveedores>
{
    public ServicioProveedores(IDbContextFactory<TicketsDbContext> fabrica, ILogger<ServicioProveedores> log)
        : base(fabrica, log) { }

    public async Task<MensajeDeRespuesta<List<GetProveedor>>> GetAll()
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                return await contexto.Proveedor
                    .Include(p => p.FkCategoriaNavigation)
                    .Where(p => p.Activo)
                    .Select(p => new GetProveedor
                    {
                        Id = p.Id,
                        FkCategoria = p.FkCategoria,
                        Nombre = p.Nombre,
                        NomCategoria = p.FkCategoriaNavigation.Nombre
                    })
                    .AsNoTracking()
                    .ToListAsync();
            },
            mensajeDeExito: "Los proveedores fueron extraídos correctamente",
            mensajeDeError: "Hubo un error al extraer los proveedores");
    }

    public async Task<MensajeDeRespuesta<int>> CrearProveedor(CreateProveedor nuevoProveedor)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var proveedor = new Proveedor
                {
                    FkCategoria = nuevoProveedor.FkCategoria,
                    Nombre = nuevoProveedor.Nombre
                };

                await contexto.Proveedor.AddAsync(proveedor);
                await contexto.SaveChangesAsync();

                return proveedor.Id;
            },
            mensajeDeExito: "Proveedor creado correctamente",
            mensajeDeError: "Hubo un error al intentar crear el proveedor");
    }

    public async Task<MensajeDeRespuesta<bool>> EliminarProveedor(int id)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var proveedor = await contexto.Proveedor.FirstOrDefaultAsync(p => p.Id == id);

                if (proveedor is null)
                    throw new InvalidOperationException($"No se encontró el proveedor con Id {id}");

                proveedor.Activo = false;
                await contexto.SaveChangesAsync();

                return true;
            },
            mensajeDeExito: "El proveedor se eliminó correctamente",
            mensajeDeError: "Hubo un error al intentar eliminar el proveedor");
    }

    public async Task<MensajeDeRespuesta<List<GetProveedor>>> FiltrarProveedores(
        FiltroDeProveedores filtrado, List<GetProveedor> proveedores)
    {
        return await EjecutarServicio(
            () =>
            {
                IEnumerable<GetProveedor> query = proveedores;

                if (!string.IsNullOrEmpty(filtrado.Nombre))
                    query = query.Where(p => p.Nombre.Contains(filtrado.Nombre, StringComparison.OrdinalIgnoreCase));

                if (filtrado.FkCategoria != null)
                    query = query.Where(p => p.FkCategoria == filtrado.FkCategoria);

                return Task.FromResult(query.ToList());
            },
            mensajeDeExito: "Datos extraídos correctamente",
            mensajeDeError: "Hubo un error al intentar filtrar los proveedores");
    }
}