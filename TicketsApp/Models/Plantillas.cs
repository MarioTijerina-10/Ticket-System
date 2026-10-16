namespace TicketsApp.Models;

public partial class Plantilla
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Asunto { get; set; }

    public string? Descripcion { get; set; }

    public int? FkCategoria { get; set; }

    public string? Solucion { get; set; }

    public string? RegistroPropio_Razon { get; set; }

    public bool AsignadoAProvedor { get; set; } = false;

    public int? FkProvedor { get; set; }

    public string? FkUsuario { get; set; } = null!;

    public virtual Categoria FkCategoriaNavigation { get; set; } = null!;

    public virtual Proveedor FkProveedorNavigation { get; set; } = null!;

    public virtual Usuario FkUsuarioNavigation { get; set; } = null!;
}