using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketsApp.DTO.Usuarios;

//DTO para la creacion de usuarios
public class CreateUsuario
{
    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public string Rol { get; set; } = null!;

    [Required]
    public string Usuario1 { get; set; } = null!;

    [EmailAddress]
    public string? Correo { get; set; }

    [Required]
    [PasswordPropertyText]
    public string Contraseña { get; set; } = null!;

    [Required]
    public int FkUnidad { get; set; }
}
