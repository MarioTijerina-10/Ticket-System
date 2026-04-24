using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketsApp.Models;

public partial class Usuario : IdentityUser
{
    public string Nombre { get; set; } = null!;

    public int FkUnidad { get; set; }

    public bool Activo { get; set; } = true;

    public virtual Unidad FkUnidadNavigation { get; set; } = null!;

    public virtual ICollection<Ticket> TicketFkAnalistaNavigations { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketFkUsuarioNavigations { get; set; } = new List<Ticket>();
}
