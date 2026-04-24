using System;
using System.Collections.Generic;

namespace TicketsApp.Models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
