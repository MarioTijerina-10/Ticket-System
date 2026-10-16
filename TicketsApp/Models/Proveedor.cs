
namespace TicketsApp.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int FkCategoria { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Categoria FkCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Plantilla> Plantillas { get; set; } = new List<Plantilla>();
}