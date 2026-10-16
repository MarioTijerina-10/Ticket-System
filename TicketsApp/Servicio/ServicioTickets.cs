using Microsoft.AspNetCore.Identity;
using TicketsApp.DTO;
using TicketsApp.DTO.Comentarios;
using TicketsApp.DTO.Tickets;

namespace TicketsApp.Servicio;

public class ServicioTickets : ServicioBase<ServicioTickets>
{
    private readonly UserManager<Usuario> userManager;
    private readonly RoleManager<IdentityRole> rolManager;
    private readonly EmailQueue cola;
    private readonly PlantillasCorreo plantillasCorreo;

    public ServicioTickets(
        IDbContextFactory<TicketsDbContext> fabrica,
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> rolManager,
        EmailQueue cola,
        PlantillasCorreo plantillasCorreo,
        ILogger<ServicioTickets> log)
        : base(fabrica, log)
    {
        this.userManager = userManager;
        this.rolManager = rolManager;
        this.cola = cola;
        this.plantillasCorreo = plantillasCorreo;
    }

    public Task<MensajeDeRespuesta<List<GetTickets>>> GetAll()
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            var respuesta = await Contexto.Tickets
                .Include(x => x.FkAnalistaNavigation)
                .Include(x => x.FkProveedorNavigation)
                .Include(x => x.FkUnidadNavigation)
                .Include(x => x.FkCategoriaNavigation)
                .Include(x => x.FkUsuarioNavigation).ThenInclude(u => u.FkDepartamentoNavigation)
                .OrderByDescending(t => t.FechaInicio)
                .ToListAsync();

            List<GetTickets> tickets = new();

            foreach (var x in respuesta)
            {
                GetTickets ticket = new()
                {
                    Id = x.Id,
                    Anydesk = x.Anydesk!,
                    Asunto = x.Asunto,
                    Descripcion = x.Descripcion!,
                    FechaCierre = x.FechaCierre,
                    FechaCierreParcial = x.FechaCierreParcial,
                    FechaTomado = x.FechaTomado,
                    FechaInicio = x.FechaInicio,
                    FkAnalista = x.FkAnalista,
                    FkCategoria = x.FkCategoria,
                    FkEstatus = x.FkEstatus,
                    FkUnidad = x.FkUnidad,
                    FkUsuario = x.FkUsuario!,
                    FkDepartamento = x.FkUsuarioNavigation.FkDepartamentoNavigation.Id!,
                    Departamento = x.FkUsuarioNavigation.FkDepartamentoNavigation.Nombre,
                    Solucion = x.Solucion,
                    Categoria = x.FkCategoriaNavigation.Nombre,
                    Estatuses = (Enumerables.Estatus)x.FkEstatus,
                    Unidad = x.FkUnidadNavigation.Nombre,
                    AsignadoAProvedor = x.AsignadoAProvedor,
                    FkProvedor = x.FkProvedor,
                    CerradoPor = x.CerradoPorUsuario,
                    Duracion_Abierto_Asignado = x.Abierto_Asignado,
                    Duracion_Abierto_Cierre = x.Abierto_Cierre,
                    Duracion_Abierto_CierreParcial = x.Abierto_CierreParcial,
                    Duracion_Asignado_CierreParcial = x.Asignado_CierreParcial,
                    Duracion_CierreParcial_Cierre = x.CierreParcial_Cierre,
                    RegistradoPorAnalista = x.RegistradoPorAnalista,
                    RegistroPropio_Razon = x.RegistroPropio_Razon,
                    NomProvedor = x.FkProvedor != null ? x.FkProveedorNavigation.Nombre : null,
                    NomAnalista = x.FkAnalista != null ? x.FkAnalistaNavigation.Nombre : "N/A",
                    NomUsuario = x.FkUsuario != null ? x.FkUsuarioNavigation.Nombre : "N/A"
                };

                tickets.Add(ticket);
            }

            return tickets;
        },
        "Los tickets fueron extraidos correctamente",
        "Hubo un error al extraer los tickets");
    }

    public Task<MensajeDeRespuesta<List<GetTickets>>> GetTicketsByUsuario(string IdUsuario)
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();

            var tickets = (await GetAll()).data ?? new();
            var TicketsPorUsuario = tickets.Where(t => t.FkUsuario == IdUsuario).ToList();

            return TicketsPorUsuario;
        },
        "Los tickets fueron extraidos correctamente",
        "Hubo un error al extraer los tickets");
    }



    public async Task<MensajeDeRespuesta<GetTickets>> GetOne(int id)
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        var respuesta = await Contexto.Tickets
            .Include(t => t.FkAnalistaNavigation)
            .Include(t => t.FkProveedorNavigation)
            .Include(t => t.FkCategoriaNavigation)
            .Include(t => t.FkUsuarioNavigation).ThenInclude(u => u.FkDepartamentoNavigation)
            .FirstOrDefaultAsync(t => t.Id == id);

        // "No encontrado" es un resultado de negocio normal, no un error de sistema,
        // asi que se maneja aparte del try/catch de EjecutarServicio.
        if (respuesta == null)
        {
            return new MensajeDeRespuesta<GetTickets>
            {
                exitoso = false,
                mensaje = "No se encontro ningun ticket."
            };
        }

        return await EjecutarServicio(async () =>
        {
            GetTickets ticket = new()
            {
                Anydesk = respuesta.Anydesk!,
                Asunto = respuesta.Asunto,
                Descripcion = respuesta.Descripcion!,
                FechaCierre = respuesta.FechaCierre,
                FechaCierreParcial = respuesta.FechaCierreParcial,
                FechaInicio = respuesta.FechaInicio,
                FechaTomado = respuesta.FechaTomado,
                FkAnalista = respuesta.FkAnalista,
                FkCategoria = respuesta.FkCategoria,
                FkEstatus = respuesta.FkEstatus,
                FkUnidad = respuesta.FkUnidad,
                FkUsuario = respuesta.FkUsuario!,
                FkDepartamento = respuesta.FkUsuarioNavigation.FkDepartamentoNavigation.Id,
                Departamento = respuesta.FkUsuarioNavigation.FkDepartamentoNavigation.Nombre,
                Solucion = respuesta.Solucion,
                NomUsuario = respuesta.FkUsuarioNavigation.Nombre,
                Categoria = respuesta.FkCategoriaNavigation.Nombre,
                Estatuses = (Enumerables.Estatus)respuesta.FkEstatus,
                AsignadoAProvedor = respuesta.AsignadoAProvedor,
                FkProvedor = respuesta.FkProvedor,
                Duracion_Abierto_Asignado = respuesta.Abierto_Asignado,
                Duracion_Abierto_Cierre = respuesta.Abierto_Cierre,
                Duracion_Abierto_CierreParcial = respuesta.Abierto_CierreParcial,
                Duracion_Asignado_CierreParcial = respuesta.Asignado_CierreParcial,
                Duracion_CierreParcial_Cierre = respuesta.CierreParcial_Cierre,
                CerradoPor = respuesta.CerradoPorUsuario,
                RegistradoPorAnalista = respuesta.RegistradoPorAnalista,
                RegistroPropio_Razon = respuesta.RegistroPropio_Razon,
                NomAnalista = respuesta.FkAnalista != null ? respuesta.FkAnalistaNavigation.Nombre : "N/A"
            };

            if (respuesta.FkProvedor != null)
                ticket.NomProvedor = respuesta.FkProveedorNavigation.Nombre;

            return ticket;
        },
        "El ticket fue extraido exitosamente.",
        "Hubo un error al extraer el ticket");
    }

    public Task<MensajeDeRespuesta<Ticket>> Createticket(CrearTickets datos, CrearComentariosTickets nuevoComentario)
    {
        string mensajeDeExito;
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            Ticket ticket = new();

            Contexto.Entry(ticket).CurrentValues.SetValues(datos);
            await Contexto.Tickets.AddAsync(ticket);
            await Contexto.SaveChangesAsync();
            await Contexto.Entry(ticket).ReloadAsync();

            ComentarioTicket comentario = new()
            {
                Contenido = nuevoComentario.Contenido,
                FkTicket = ticket.Id,
                FkUsuario = nuevoComentario.FkUsuario
            };
            await Contexto.Comentarios.AddAsync(comentario);
            await Contexto.SaveChangesAsync();

            mensajeDeExito = $"Se registro el Ticket N° {ticket.Id}";
            return ticket;
        },
        $"",
        "Hubo un error al intentar crear el ticket",
        ticket => $"Se registro el Ticket N° {ticket.Id}");
    }

    public Task<MensajeDeRespuesta<Ticket>> AsignarAProvedor(int id, int? provedor, bool estado)
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            var ticket = await Contexto.Tickets.FirstOrDefaultAsync(t => t.Id == id);

            ticket!.AsignadoAProvedor = estado;
            ticket.FkProvedor = provedor;
            await Contexto.SaveChangesAsync();

            return ticket;
        },
        "El proveedor fue asignado correctamente",
        "Hubo un error al intentar editar el ticket", null);
    }

    public Task<MensajeDeRespuesta<Ticket>> AsignarTicket(int id, string analista)
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            var ticket = await Contexto.Tickets.FirstOrDefaultAsync(t => t.Id == id);

            ticket!.FkEstatus = 2;
            ticket.FkAnalista = analista;
            ticket.FechaTomado ??= DateTime.Now;

            await Contexto.SaveChangesAsync();

            return ticket;
        },
        "El ticket se asigno correctamente",
        "Hubo un error al intentar asignar el ticket");
    }

    public async Task<MensajeDeRespuesta<Ticket>> UpdateTickets(UpdateTickets datos, int id)
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        var ticket = await Contexto.Tickets.Include(t => t.FkUsuarioNavigation).FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
        {
            return new MensajeDeRespuesta<Ticket>
            {
                exitoso = false,
                mensaje = "No se encontro ningun ticket.",
                data = ticket
            };
        }

        return await EjecutarServicio(async () =>
        {
            ticket.FechaCierreParcial = datos.FkEstatus == 3 ? DateTime.Now : null;
            ticket.FechaCierreParcial = datos.FkEstatus == 2 ? null : ticket.FechaCierreParcial;
            ticket.FkCategoria = datos.FkCategoria;
            ticket.FkEstatus = datos.FkEstatus;
            ticket.Solucion = datos.Solucion;

            if (datos.FkEstatus == 3)
            {
                var correoUsuario = ticket.FkUsuarioNavigation.Email;
                if (correoUsuario != null)
                {
                    // var html = await plantillasCorreo.TicketCerradoParcial(ticket.Id, ticket.Asunto);
                    // await cola.QueueEmailAsync(new EmailRequest
                    // {
                    //     Para = correoUsuario,
                    //     Asunto = ticket.Asunto,
                    //     html = html
                    // });
                }
            }

            await Contexto.SaveChangesAsync();
            return ticket;
        },
        "El ticket fue actualizado correctamente",
        "Hubo un error al intentar actualizar el ticket");
    }

    public Task<MensajeDeRespuesta<Ticket>> EditarEstatus(int id, int estatus)
    {
        return EjecutarServicio(async () =>
        {
            var Contexto = await fabrica.CreateDbContextAsync();
            var ticket = await Contexto.Tickets.Include(t => t.FkAnalistaNavigation).FirstOrDefaultAsync(t => t.Id == id);

            ticket!.FkEstatus = estatus;
            var correoAnalista = ticket.FkAnalistaNavigation.Email;

            switch (estatus)
            {
                case 2:
                    ticket.FechaCierreParcial = null;
                    ticket.FechaCierre = null;

                    if (correoAnalista != null)
                    {
                        var html = await plantillasCorreo.TicketReasignado(ticket.Id, ticket.Asunto);
                        await cola.QueueEmailAsync(new EmailRequest
                        {
                            Para = correoAnalista,
                            Asunto = ticket.Asunto,
                            html = html
                        });
                    }
                    break;

                case 3:
                    ticket.FechaCierreParcial = DateTime.Now;
                    ticket.FechaCierre = null;
                    break;

                case 4:
                    ticket.FechaCierre = DateTime.Now;
                    ticket.CerradoPorUsuario = true;

                    if (correoAnalista != null)
                    {
                        // var html = await plantillasCorreo.TicketCerrado(ticket.Id, ticket.Asunto);
                        // await cola.QueueEmailAsync(new EmailRequest
                        // {
                        //     Para = correoAnalista,
                        //     Asunto = ticket.Asunto,
                        //     html = html
                        // });
                    }
                    break;
            }

            await Contexto.SaveChangesAsync();
            return ticket;
        },
        "Ticket editado exitosamente",
        "Hubo un error al intentar editar el ticket");
        // Nota: el Console.WriteLine del original fue reemplazado por el logger
        // centralizado en la clase base (mismo canal que el resto del servicio).
    }

    public Task<MensajeDeRespuesta<List<GetTickets>>> filtrarTickets(FiltroDeTickets filtrado, List<GetTickets> tickets)
    {
        return EjecutarServicio(() =>
        {
            IEnumerable<GetTickets> query = tickets;

            if (filtrado.RegistradoPorAnalista != null)
                query = query.Where(t => t.RegistradoPorAnalista == filtrado.RegistradoPorAnalista.Value);

            if (!string.IsNullOrEmpty(filtrado.NomUsuario))
                query = query.Where(t => t.NomUsuario?.Contains(filtrado.NomUsuario) == true);

            if (filtrado.FkAnalista != null)
                query = query.Where(t => t.FkAnalista == filtrado.FkAnalista);

            if (filtrado.FkEstatus != null)
                query = query.Where(t => t.FkEstatus == filtrado.FkEstatus);

            if (filtrado.FkCategoria != null)
                query = query.Where(t => t.FkCategoria == filtrado.FkCategoria);

            if (filtrado.FkDepartamento != null)
                query = query.Where(t => t.FkDepartamento == filtrado.FkDepartamento);

            if (filtrado.rangoDeFecha?.Start != null && filtrado.rangoDeFecha.End != null)
                query = query.Where(t => t.FechaInicio >= filtrado.rangoDeFecha.Start!.Value
                                    && t.FechaInicio <= filtrado.rangoDeFecha.End!.Value);

            query = AplicarFiltroDeTiempo(query, t => t.Duracion_Abierto_Asignado, filtrado.Duracion_Abierto_Asignado);
            query = AplicarFiltroDeTiempo(query, t => t.Duracion_Abierto_CierreParcial, filtrado.Duracion_Asignado_CierreParcial);
            query = AplicarFiltroDeTiempo(query, t => t.Duracion_Abierto_Cierre, filtrado.Duracion_CierreParcial_Cierre);
            query = AplicarFiltroDeTiempo(query, t => t.Duracion_Asignado_CierreParcial, filtrado.Duracion_Abierto_CierreParcial);
            query = AplicarFiltroDeTiempo(query, t => t.Duracion_CierreParcial_Cierre, filtrado.Duracion_Abierto_Cierre);

            return Task.FromResult(query.ToList());
        },
        "Datos extraidos correctamente",
        "Hubo un error al intentar filtrar tickets");
    }

    private static IEnumerable<GetTickets> AplicarFiltroDeTiempo(
        IEnumerable<GetTickets> query,
        Func<GetTickets, TimeSpan?> selector,
        RangoDeTiempo? rango)
    {
        if (rango == null)
            return query;

        if (rango.Final == null)
        {
            return query.Where(t =>
            {
                var duracion = selector(t);
                return duracion.HasValue && duracion.Value.TotalMinutes >= rango.Inicio.TotalMinutes;
            });
        }

        return query.Where(t =>
        {
            var duracion = selector(t);
            return duracion.HasValue
                && duracion.Value.TotalMinutes >= rango.Inicio.TotalMinutes
                && duracion.Value.TotalMinutes < rango.Final.Value.TotalMinutes;
        });
    }
}