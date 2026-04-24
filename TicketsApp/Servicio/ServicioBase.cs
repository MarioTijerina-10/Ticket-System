using System;
using Microsoft.EntityFrameworkCore;

namespace TicketsApp.Servicio;

//Clase base para servicio
public class ServicioBase
{
    public readonly IDbContextFactory<TicketsDbContext> fabrica = null!;

    public ServicioBase(IDbContextFactory<TicketsDbContext> _fabrica)
    {
        fabrica = _fabrica;
    }
}
