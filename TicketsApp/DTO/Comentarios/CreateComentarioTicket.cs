namespace TicketsApp.DTO.Comentarios;

public class CrearComentariosTickets
{
    public string Contenido { get; set; } = null!;

    public int? FkTicket { get; set; }

    public string FkUsuario { get; set; } = null!;
}