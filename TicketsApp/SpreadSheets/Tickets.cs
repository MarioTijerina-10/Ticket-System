using System;

namespace TicketsApp.DTO.Tickets;

public class TicketsRow
{
    public int Id { get; set; }

    public string FkUsuario { get; set; } = null!;

    public string? NomUsuario { get; set; }

    public string? FkAnalista { get; set; }

    public string? NomAnalista { get; set; }

    public string Asunto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int FkCategoria { get; set; }

    public Enumerables.Categorias Categoria { get; set; }

    public string? Anydesk { get; set; } = "";

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaTomado { get; set; }

    public DateTime? FechaCierreParcial { get; set; }

    public DateTime? FechaCierre { get; set; }

    public int FkEstatus { get; set; }

    public Enumerables.Estatus Estatuses { get; set; }

    public string? Solucion { get; set; } = "";

    public TimeSpan? Duracion { get; set; }

    public int FkUnidad { get; set; }

    public bool? AsignadoAProvedor { get; set; }

    public string? Provedor { get; set; } = "";

    public TimeSpan? DuracionTiempo { get; set; }

    public TimeSpan? DuracionAbierto { get; set; }

    public TimeSpan? DuracionHastaCierreParcial { get; set; }
}
