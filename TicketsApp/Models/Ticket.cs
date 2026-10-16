using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsApp.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string? FkUsuario { get; set; } = null!;

    public string? FkAnalista { get; set; }

    public string Asunto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int FkCategoria { get; set; }

    public string? Anydesk { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaTomado { get; set; }

    public DateTime? FechaCierreParcial { get; set; }

    public DateTime? FechaCierre { get; set; }

    public int FkEstatus { get; set; }

    public string? Solucion { get; set; }

    public int? Duracion { get; set; }

    public int? TiempoAbierto { get; set; }

    public int? TiempoHastaCierreParcial { get; set; }

    public int? TiempoHastaCierre { get; set; }

    public int? Duracion_Abierto_CierreParcial { get; set; }

    public int? Duracion_Abierto_Cierre { get; set; }

    public int FkUnidad { get; set; }

    public bool AsignadoAProvedor { get; set; } = false;

    public int? FkProvedor { get; set; }

    public bool RegistradoPorAnalista { get; set; } = false;

    public string? RegistroPropio_Razon { get; set; }

    public bool? CerradoPorUsuario { get; set; } = null;

    public virtual Usuario FkAnalistaNavigation { get; set; } = null!;

    public virtual Categoria FkCategoriaNavigation { get; set; } = null!;

    public virtual Proveedor FkProveedorNavigation { get; set; } = null!;

    public virtual Estatus FkEstatusNavigation { get; set; } = null!;

    public virtual Unidad FkUnidadNavigation { get; set; } = null!;

    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ComentarioTicket> ComentariosTickets { get; set; } = new List<ComentarioTicket>();

    public virtual ICollection<Notificacion> NotificacionesTickets { get; set; } = new List<Notificacion>();

    [NotMapped]
    public TimeSpan? Abierto_Asignado => TiempoAbierto.HasValue ? TimeSpan.FromMinutes(TiempoAbierto.Value) : null;

    [NotMapped]
    public TimeSpan? Asignado_CierreParcial => TiempoHastaCierreParcial.HasValue ? TimeSpan.FromMinutes(TiempoHastaCierreParcial.Value) : null;

    [NotMapped]
    public TimeSpan? CierreParcial_Cierre => TiempoHastaCierre.HasValue ? TimeSpan.FromMinutes(TiempoHastaCierre.Value) : null;

    [NotMapped]
    public TimeSpan? Abierto_CierreParcial => Duracion_Abierto_CierreParcial.HasValue ? TimeSpan.FromMinutes(Duracion_Abierto_CierreParcial.Value) : null;

    [NotMapped]
    public TimeSpan? Abierto_Cierre => Duracion.HasValue ? TimeSpan.FromMinutes(Duracion.Value) : null;
}
