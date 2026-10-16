namespace TicketsApp.DTO.Usuarios;

public record UserData(string Nombre, string Id, int Unidad, int Departamento, string Correo, ClaimsPrincipal EstadoUsuario);
