
namespace TicketsApp.DTO.Proveedores;

public class CreateProveedor
{
    public string Nombre { get; set; } = null!;

    public int FkCategoria { get; set; }
}