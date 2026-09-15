using System;
using System.Collections.Generic;

namespace TicketsApp.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; } = true;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
}
