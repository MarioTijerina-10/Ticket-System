using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;

namespace TicketsApp.Servicio;

public class ServicioUnidades : ServicioBase<ServicioUnidades>
{
    public ServicioUnidades(IDbContextFactory<TicketsDbContext> fabrica, ILogger<ServicioUnidades> log)
        : base(fabrica, log) { }

    public async Task<MensajeDeRespuesta<List<Unidad>>> GetAllUnidades()
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                return await contexto.Unidades
                    .Where(u => u.Activo)
                    .AsNoTracking()
                    .ToListAsync();
            },
            mensajeDeExito: "Unidades obtenidas correctamente",
            mensajeDeError: "Hubo un error al obtener las unidades");
    }

    public async Task<MensajeDeRespuesta<int>> CrearUnidad(Unidad unidadNueva)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                await contexto.Unidades.AddAsync(unidadNueva);
                await contexto.SaveChangesAsync();

                return unidadNueva.Id;
            },
            mensajeDeExito: "La unidad se añadió correctamente",
            mensajeDeError: "Hubo un error al intentar añadir la unidad");
    }

    public async Task<MensajeDeRespuesta<bool>> Deshabilitar(int id)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var unidad = await contexto.Unidades.FirstOrDefaultAsync(u => u.Id == id);

                if (unidad is null)
                    throw new InvalidOperationException($"No se encontró la unidad con Id {id}");

                unidad.Activo = false;
                await contexto.SaveChangesAsync();

                return true;
            },
            mensajeDeExito: "La unidad se eliminó correctamente",
            mensajeDeError: "Hubo un error al intentar eliminar la unidad");
    }

    public async Task<MensajeDeRespuesta<int>> GetPrimerFk()
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();
                return (await contexto.Unidades.FirstAsync()).Id;
            },
            mensajeDeExito: "El departamento se extrajo el Id correctamente",
            mensajeDeError: "Hubo un error al intentar extraer el Id del departamento"
        );
    }
}