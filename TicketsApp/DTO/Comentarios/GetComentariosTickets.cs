namespace TicketsApp.DTO.Comentarios;

public class GetComentariosTickets
{
    public string Contenido { get; set; } = null!;

    public int FkTicket { get; set; }

    public string FkUsuario { get; set; } = null!;

    public string NomUsuario { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }
}