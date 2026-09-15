using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;
using TicketsApp.DTO.Departamentos;

namespace TicketsApp.Servicio;

public class ServicioDepartamentos : ServicioBase<ServicioDepartamentos>
{
    public ServicioDepartamentos(IDbContextFactory<TicketsDbContext> fabrica, ILogger<ServicioDepartamentos> log)
        : base(fabrica, log) { }

    public async Task<MensajeDeRespuesta<List<GetDepartamentos>>> GetAll()
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                return await contexto.Departamentos
                    .Where(d => d.Activo)
                    .Select(d => new GetDepartamentos
                    {
                        Id = d.Id,
                        Nombre = d.Nombre
                    })
                    .AsNoTracking()
                    .ToListAsync();
            },
            mensajeDeExito: "Departamentos obtenidos correctamente",
            mensajeDeError: "Hubo un error al obtener los departamentos");
    }

    public async Task<MensajeDeRespuesta<int>> Crear(string nombre)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var departamentoNuevo = new Departamento { Nombre = nombre };

                contexto.Departamentos.Add(departamentoNuevo);
                await contexto.SaveChangesAsync();

                return departamentoNuevo.Id;
            },
            mensajeDeExito: "El departamento se añadió correctamente",
            mensajeDeError: "Hubo un error al intentar añadir el departamento");
    }

    public async Task<MensajeDeRespuesta<bool>> Eliminar(int id)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var departamento = await contexto.Departamentos.FirstOrDefaultAsync(d => d.Id == id);

                if (departamento is null)
                    throw new InvalidOperationException($"No se encontró el departamento con Id {id}");

                departamento.Activo = false;
                await contexto.SaveChangesAsync();

                return true;
            },
            mensajeDeExito: "El departamento se eliminó correctamente",
            mensajeDeError: "Hubo un error al intentar eliminar el departamento");
    }
}