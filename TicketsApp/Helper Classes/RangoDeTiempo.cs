using DocumentFormat.OpenXml.InkML;

namespace TicketsApp.Helper_Classes;

public class RangoDeTiempo
{
    public TimeSpan Inicio { get; set; }

    public TimeSpan? Final { get; set; }
}