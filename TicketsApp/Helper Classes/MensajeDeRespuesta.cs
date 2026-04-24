using System;

namespace TicketsApp.Helper_Classes;

public class MensajeDeRespuesta
{
    public bool exitoso { get; set; }
    public string? mensaje { get; set; }
}

public class MensajeDeRespuesta<T> : MensajeDeRespuesta
{
    public T? data { get; set; }
}
