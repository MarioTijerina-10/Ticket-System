namespace TicketsApp.DTO;

public class CrearTickets
{
    public string? FkUsuario { get; set; } = null!;

    public string? FkAnalista { get; set; }

    public string Asunto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int FkCategoria { get; set; }

    public string? Anydesk { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaTomado { get; set; } = null;

    public DateTime? FechaCierreParcial { get; set; } = null;

    public DateTime? FechaCierre { get; set; } = null;

    public int FkEstatus { get; set; }

    public string? Solucion { get; set; }

    public int FkUnidad { get; set; }

    public bool AsignadoAProvedor { get; set; }

    public int? FkProvedor { get; set; } = null;

    public bool RegistradoPorAnalista { get; set; }

    public string? RegistroPropio_Razon { get; set; }
}
