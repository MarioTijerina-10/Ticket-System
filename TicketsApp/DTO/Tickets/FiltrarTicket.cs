using System;
using MudBlazor;

namespace TicketsApp.DTO.Tickets;

public class FiltroDeTickets
{
    public bool? RegistradoPorAnalista { get; set; }

    public string? NomUsuario { get; set; }

    public string? FkAnalista { get; set; }

    public int? FkEstatus { get; set; }

    public int? FkCategoria { get; set; }

    public int? FkDepartamento { get; set; }

    public DateRange rangoDeFecha { get; set; } = null!;

    public RangoDeTiempo? Duracion_Abierto_Asignado { get; set; }

    public RangoDeTiempo? Duracion_Asignado_CierreParcial { get; set; }

    public RangoDeTiempo? Duracion_CierreParcial_Cierre { get; set; }

    public RangoDeTiempo? Duracion_Abierto_CierreParcial { get; set; }

    public RangoDeTiempo? Duracion_Abierto_Cierre { get; set; }
}