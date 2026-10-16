public class Notificacion
{
    public int Id { get; set; }
    public string FkAnalista { get; set; } = "";
    public string FkUsuario { get; set; } = "";
    public int FkTicket { get; set; }
    public string Mensaje { get; set; } = "";
    public bool Leida { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public virtual Ticket FkTicketNavigation { get; set; } = null!;
    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;
    public virtual Usuario FkAnalistaNavigation { get; set; } = null!;
}