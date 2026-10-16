namespace TicketsApp.DTO.Plantillas;

public class CrearPlantilla
{
    public string? Nombre { get; set; }

    public string? Asunto { get; set; }

    public string? Descripcion { get; set; }

    public int? FkCategoria { get; set; }

    public string? Solucion { get; set; }

    public string? RegistroPropio_Razon { get; set; }

    public bool AsignadoAProvedor { get; set; } = false;

    public int? FkProvedor { get; set; }

    public string? FkUsuario { get; set; }
}