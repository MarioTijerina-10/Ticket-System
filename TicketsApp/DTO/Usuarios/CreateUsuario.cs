using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketsApp.DTO.Usuarios;

//DTO para la creacion de usuarios
public class CreateUsuario
{
    public string Nombre { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public int FkDepartamento { get; set; }

    public string? Correo { get; set; }

    public string Contraseña { get; set; } = null!;

    public int FkUnidad { get; set; }
}
