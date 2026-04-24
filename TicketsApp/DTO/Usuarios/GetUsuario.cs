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

    public IList<string> Rol { get; set; } = null!;

    public string? Correo { get; set; }

    public string Contraseña { get; set; } = null!;

    public Enumerables.Unidades Unidad { get; set; }

}
