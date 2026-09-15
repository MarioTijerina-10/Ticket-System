namespace TicketsApp.Models;

public partial class ComentarioTicket
{
    public int Id { get; set; }

    public string Contenido { get; set; } = null!;

    public int FkTicket { get; set; }

    public string FkUsuario { get; set; } = null!;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public virtual Ticket FkTicketNavigation { get; set; } = null!;

    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;
}