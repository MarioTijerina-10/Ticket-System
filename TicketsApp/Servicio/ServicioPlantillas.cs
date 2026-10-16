using TicketsApp.DTO.Plantillas;

namespace TicketsApp.Servicio;

public class ServicioPlantillas : ServicioBase<ServicioPlantillas>
{
    public ServicioPlantillas(
        IDbContextFactory<TicketsDbContext> fabrica,
        ILogger<ServicioPlantillas> log)
        : base(fabrica, log) { }

    public Task<MensajeDeRespuesta<List<GetPlantilla>>> GetAll(string fkUsuario)
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            var respuesta = await Contexto.Plantillas
                .Include(x => x.FkCategoriaNavigation)
                .Include(x => x.FkProveedorNavigation).Where(p => p.FkUsuario == fkUsuario)
                .ToListAsync();

            List<GetPlantilla> plantillas = new();
            Console.WriteLine($"Cantidad de plantillas: {respuesta.Count()}");

            foreach (var x in respuesta)
            {
                GetPlantilla plantilla = new()
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Asunto = x.Asunto,
                    Descripcion = x.Descripcion!,
                    FkCategoria = x.FkCategoria,
                    NomCategoria = x.FkCategoriaNavigation?.Nombre ?? null,
                    Solucion = x.Solucion,
                    AsignadoAProvedor = x.AsignadoAProvedor,
                    FkProvedor = x.FkProvedor,
                    RegistroPropio_Razon = x.RegistroPropio_Razon,
                    NomProveedor = x.FkProveedorNavigation?.Nombre ?? null,
                    FkUsuario = x.FkUsuario,
                };

                plantillas.Add(plantilla);
            }

            return plantillas;
        },
        "Las plantillas fueron extraidos correctamente",
        "Hubo un error al extraer las plantillas");
    }

    public async Task<MensajeDeRespuesta<GetPlantilla>> GetOne(int id)
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        var respuesta = await Contexto.Plantillas
            .Include(t => t.FkProveedorNavigation)
            .Include(t => t.FkCategoriaNavigation)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (respuesta == null)
        {
            return new MensajeDeRespuesta<GetPlantilla>
            {
                exitoso = false,
                mensaje = "No se encontro ninguna plantilla."
            };
        }

        return await EjecutarServicio(async () =>
        {
            GetPlantilla plantilla = new()
            {
                Id = respuesta.Id,
                Nombre = respuesta.Nombre,
                Asunto = respuesta.Asunto,
                Descripcion = respuesta.Descripcion!,
                FkCategoria = respuesta.FkCategoria,
                NomCategoria = respuesta.FkCategoriaNavigation.Nombre,
                FkUsuario = respuesta.FkUsuario!,
                Solucion = respuesta.Solucion,
                AsignadoAProvedor = respuesta.AsignadoAProvedor,
                FkProvedor = respuesta.FkProvedor,
                NomProveedor = respuesta.FkProveedorNavigation.Nombre,
                RegistroPropio_Razon = respuesta.RegistroPropio_Razon,
            };

            return plantilla;
        },
        "La plantilla fue extraido exitosamente.",
        "Hubo un error al extraer la plantilla");
    }

    public Task<MensajeDeRespuesta<Plantilla>> CreatePlantilla(CrearPlantilla datos)
    {
        string mensajeDeExito;
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            Plantilla plantilla = new();

            Contexto.Entry(plantilla).CurrentValues.SetValues(datos);
            await Contexto.Plantillas.AddAsync(plantilla);
            await Contexto.SaveChangesAsync();

            mensajeDeExito = $"Se registro la plantilla correctamente";
            return plantilla;
        },
        $"Se registro la plantilla correctamente",
        "Hubo un error al intentar crear la plantilla");
    }

    public async Task<MensajeDeRespuesta<Plantilla>> UpdatePlantilla(UpdatePlantilla datos, int id)
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        var plantilla = await Contexto.Plantillas.FirstOrDefaultAsync(t => t.Id == id);

        if (plantilla == null)
        {
            return new MensajeDeRespuesta<Plantilla>
            {
                exitoso = false,
                mensaje = "No se encontro ninguna plantilla.",
                data = plantilla
            };
        }

        return await EjecutarServicio(async () =>
        {
            plantilla.AsignadoAProvedor = datos.AsignadoAProvedor;
            plantilla.Asunto = datos.Asunto;
            plantilla.Descripcion = datos.Descripcion;
            plantilla.FkProvedor = datos.FkProvedor;
            plantilla.Nombre = datos.Nombre;
            plantilla.RegistroPropio_Razon = datos.RegistroPropio_Razon;
            plantilla.FkCategoria = datos.FkCategoria;
            plantilla.Solucion = datos.Solucion;

            await Contexto.SaveChangesAsync();
            return plantilla;
        },
        "La plantilla fue actualizado correctamente",
        "Hubo un error al intentar actualizar la plantilla");
    }

    public async Task<MensajeDeRespuesta<Plantilla>> EliminarPlantilla(int id)
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        var plantilla = await Contexto.Plantillas.FirstOrDefaultAsync(p => p.Id == id);

        if (plantilla == null)
        {
            return new MensajeDeRespuesta<Plantilla>()
            {
                mensaje = "No se encontro ninguna plantilla",
                exitoso = false,
                data = null
            };
        }

        return await EjecutarServicio(async () =>
        {
            Contexto.Remove(plantilla);
            await Contexto.SaveChangesAsync();
            return plantilla;
        },
        "Se elimino la plantilla correctamente",
        "No se pudo eliminar la plantilla"
        );
    }
}