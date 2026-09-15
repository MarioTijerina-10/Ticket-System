using System;
using MudBlazor;

namespace TicketsApp.DTO.Proveedores;

public class FiltroDeProveedores
{
    public string? Nombre { get; set; }

    public int? FkCategoria { get; set; }
}