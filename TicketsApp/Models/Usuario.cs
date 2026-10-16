using Microsoft.AspNetCore.Identity;

namespace TicketsApp.Models;

public partial class Usuario : IdentityUser
{
    public string Nombre { get; set; } = null!;

    public string? ContraseñaVisible { get; set; } = null!;

    public int FkUnidad { get; set; }

    public bool Activo { get; set; } = true;

    public int FkDepartamento { get; set; }

    public virtual Unidad FkUnidadNavigation { get; set; } = null!;

    public virtual Departamento FkDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<Ticket> TicketFkAnalistaNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketFkUsuarioNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<ComentarioTicket> ComentarioTicketFkUsuarioNavigations { get; set; } = new List<ComentarioTicket>();

    public virtual ICollection<Plantilla> Plantillas { get; set; } = new List<Plantilla>();

    public virtual ICollection<Notificacion> NotificacionFkAnalistaNavigations { get; set; } = new List<Notificacion>();

    public virtual ICollection<Notificacion> NotificacionFkUsuarioNavigations { get; set; } = new List<Notificacion>();

    public string? TiempoDeCierre_Promedio(DateTime inicio, DateTime final) =>
    TicketFkAnalistaNavigations.Any(t => t.Asignado_CierreParcial != null && t.Asignado_CierreParcial.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final) ?
    TimeSpan.FromMinutes(TicketFkAnalistaNavigations.Where(t => t.FkAnalista == Id && t.Asignado_CierreParcial.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final).Average(t => t.Asignado_CierreParcial!.Value.TotalMinutes)).ToString(@"d'd 'hh'h 'mm'm'")
    : null;

    public string? TiempoDeRespuesta_Promedio(DateTime inicio, DateTime final) =>
    TicketFkAnalistaNavigations.Any(t => t.Abierto_Asignado != null && t.Abierto_Asignado.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final) ?
    TimeSpan.FromMinutes(TicketFkAnalistaNavigations.Where(t => t.FkAnalista == Id && t.Abierto_Asignado.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final).Average(t => t.Abierto_Asignado!.Value.TotalMinutes)).ToString(@"d'd 'hh'h 'mm'm'")
    : null;

    public string? TiempoTotal_Promedio(DateTime inicio, DateTime final) =>
    TicketFkAnalistaNavigations.Any(t => t.Abierto_Cierre != null && t.Abierto_Cierre.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final) ?
    TimeSpan.FromMinutes(TicketFkAnalistaNavigations.Where(t => t.FkAnalista == Id && t.Abierto_Cierre.HasValue && t.FechaInicio >= inicio && t.FechaInicio <= final).Average(t => t.Abierto_Cierre!.Value.TotalMinutes)).ToString(@"d'd 'hh'h 'mm'm'")
    : null;
}
