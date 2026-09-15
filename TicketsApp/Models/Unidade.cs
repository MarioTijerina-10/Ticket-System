using System;
using System.Collections.Generic;

namespace TicketsApp.Models;

public partial class Unidad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; } = true;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
