using System;

namespace TicketsApp.DTO.Tickets;

public class UpdateTickets
{

    public int FkCategoria { get; set; }

    public int FkEstatus { get; set; }

    public DateTime? FechaCierreParcial { get; set; }

    public DateTime? FechaCierre { get; set; }


    public string? Solucion { get; set; }
}
