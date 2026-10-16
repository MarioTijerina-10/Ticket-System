namespace TicketsApp.DTO.Usuarios;

public class FiltroDeUsuarios
{
    public string? Nombre { get; set; }

    public string? Rol { get; set; }

    public int? FkUnidad { get; set; }

    public int? FkDepartamento { get; set; }
}