using System;
using System.Collections.Generic;

namespace TicketsApp.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public string FkUsuario { get; set; } = null!;

    public string FkAnalista { get; set; } = null!;

    public string Asunto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int FkCategoria { get; set; }

    public string? Anydesk { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public int FkEstatus { get; set; }

    public string? Solucion { get; set; }

    public DateTime? Duracion { get; set; }

    public int FkUnidad { get; set; }

    public virtual Usuario FkAnalistaNavigation { get; set; } = null!;

    public virtual Categoria FkCategoriaNavigation { get; set; } = null!;

    public virtual Estatus FkEstatusNavigation { get; set; } = null!;

    public virtual Unidad FkUnidadNavigation { get; set; } = null!;

    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;
}
