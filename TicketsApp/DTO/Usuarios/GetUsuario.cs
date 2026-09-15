using System;
using Microsoft.AspNetCore.Identity;
using TicketsApp.Enums;

namespace TicketsApp.DTO.Usuarios;

//DTO para el uso de usuarios extraidos
public class GetUsuario
{
    public string Id { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public int FkDepartamento { get; set; }

    public string Departamento { get; set; } = null!;

    public string NomDepartamento { get; set; } = null!;

    public IList<string> Rol { get; set; } = null!;

    public string? Correo { get; set; }

    public string Contraseña { get; set; } = null!;

    public string ContraseñaVisible { get; set; } = null!;

    public string Unidad { get; set; } = null!;

    public int FkUnidad { get; set; }
}
