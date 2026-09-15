using System;

namespace TicketsApp.DTO.Tickets;

public class GetTickets
{
    public int Id { get; set; }

    public string FkUsuario { get; set; } = null!;

    public string? NomUsuario { get; set; }

    public string? Departamento { get; set; }

    public int FkDepartamento { get; set; }

    public string? FkAnalista { get; set; }

    public string? NomAnalista { get; set; }

    public string Asunto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int FkCategoria { get; set; }

    public string Categoria { get; set; } = null!;

    public string? Anydesk { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaTomado { get; set; }

    public DateTime? FechaCierreParcial { get; set; }

    public DateTime? FechaCierre { get; set; }

    public int FkEstatus { get; set; }

    public Enumerables.Estatus Estatuses { get; set; }

    public string? Solucion { get; set; }

    public int FkUnidad { get; set; }

    public string Unidad { get; set; } = null!;

    public bool? AsignadoAProvedor { get; set; }

    public bool RegistradoPorAnalista { get; set; }

    public string? RegistroPropio_Razon { get; set; }

    public bool? CerradoPor { get; set; } = null!;

    public int? FkProvedor { get; set; }

    public string? NomProvedor { get; set; }

    public TimeSpan? Duracion_Abierto_Asignado { get; set; }

    public TimeSpan? Duracion_Asignado_CierreParcial { get; set; }

    public TimeSpan? Duracion_CierreParcial_Cierre { get; set; }

    public TimeSpan? Duracion_Abierto_CierreParcial { get; set; }

    public TimeSpan? Duracion_Abierto_Cierre { get; set; }
}
