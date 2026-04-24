using System;

namespace TicketsApp.DTO.Usuarios;

public class UpdateUsuario
{
    public string Nombre { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string? Correo { get; set; }

    public string Rol { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int FkUnidad { get; set; }
}
