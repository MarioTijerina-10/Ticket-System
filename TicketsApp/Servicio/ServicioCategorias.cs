using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;
using TicketsApp.DTO.Categorias;

namespace TicketsApp.Servicio;

public class ServicioCategorias : ServicioBase<ServicioCategorias>
{
    public ServicioCategorias(IDbContextFactory<TicketsDbContext> fabrica, ILogger<ServicioCategorias> log)
        : base(fabrica, log) { }

    public async Task<MensajeDeRespuesta<List<GetCategorias>>> GetCategorias()
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                return await contexto.Categorias
                    .Where(c => c.Activo)
                    .Select(c => new GetCategorias
                    {
                        Id = c.Id,
                        Nombre = c.Nombre
                    })
                    .AsNoTracking()
                    .ToListAsync();
            },
            mensajeDeExito: "Categorias obtenidas correctamente",
            mensajeDeError: "Hubo un error al obtener las categorias");
    }

    public async Task<MensajeDeRespuesta<int>> CrearCategoria(string nombre)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var categoriaNueva = new Categoria { Nombre = nombre };

                await contexto.Categorias.AddAsync(categoriaNueva);
                await contexto.SaveChangesAsync();

                return categoriaNueva.Id;
            },
            mensajeDeExito: "La categoria se añadió correctamente",
            mensajeDeError: "Hubo un error al intentar añadir la categoria");
    }

    public async Task<MensajeDeRespuesta<bool>> EliminarCategoria(int id)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                var categoria = await contexto.Categorias.FirstOrDefaultAsync(c => c.Id == id);

                if (categoria is null)
                    throw new InvalidOperationException($"No se encontró la categoria con Id {id}");

                categoria.Activo = false;
                await contexto.SaveChangesAsync();

                return true;
            },
            mensajeDeExito: "La categoria se eliminó correctamente",
            mensajeDeError: "Hubo un error al eliminar la categoria");
    }
}