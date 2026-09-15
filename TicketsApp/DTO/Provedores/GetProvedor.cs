
namespace TicketsApp.DTO.Proveedores;

public class GetProveedor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NomCategoria { get; set; } = null!;

    public int FkCategoria { get; set; }

    public bool Activo { get; set; } = true;
}