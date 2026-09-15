using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;
using TicketsApp.DTO.Comentarios;

namespace TicketsApp.Servicio;

public class ServicioComentarios : ServicioBase<ServicioComentarios>
{
    private readonly UserManager<Usuario> userManager;
    private readonly RoleManager<IdentityRole> rolManager;

    public ServicioComentarios(
        IDbContextFactory<TicketsDbContext> fabrica,
        ILogger<ServicioComentarios> log,
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> rolManager)
        : base(fabrica, log)
    {
        this.userManager = userManager;
        this.rolManager = rolManager;
    }

    public async Task<MensajeDeRespuesta<List<GetComentariosTickets>>> GetComentarios_Ticket(int id)
    {
        return await EjecutarServicio(
            async () =>
            {
                await using var contexto = await fabrica.CreateDbContextAsync();

                return await contexto.Comentarios
                    .Include(c => c.FkUsuarioNavigation)
                    .Where(c => c.FkTicket == id)
                    .OrderBy(c => c.FechaCreacion)
                    .Select(c => new GetComentariosTickets
                    {
                        Contenido = c.Contenido,
                        FkTicket = c.FkTicket,
                        FkUsuario = c.FkUsuario,
                        NomUsuario = c.FkUsuarioNavigation.Nombre,
                        FechaCreacion = c.FechaCreacion
                    })
                    .AsNoTracking()
                    .ToListAsync();
            },
            mensajeDeExito: "Comentarios obtenidos correctamente",
            mensajeDeError: $"Hubo un error al obtener los comentarios del ticket {id}");
    }

    public async Task<MensajeDeRespuesta<int>> CrearComentario(CrearComentariosTickets comentario)
    {
        return await EjecutarServicio(
            async () =>
            {
                if (comentario.FkTicket is null)
                    throw new ArgumentException("El comentario debe estar asociado a un ticket");

                await using var contexto = await fabrica.CreateDbContextAsync();

                var comentarioNuevo = new ComentarioTicket
                {
                    Contenido = comentario.Contenido,
                    FkTicket = comentario.FkTicket.Value,
                    FkUsuario = comentario.FkUsuario,
                };

                await contexto.Comentarios.AddAsync(comentarioNuevo);
                await contexto.SaveChangesAsync();

                return comentarioNuevo.Id;
            },
            mensajeDeExito: "El comentario se añadió correctamente",
            mensajeDeError: "Hubo un error al intentar añadir el comentario");
    }
}