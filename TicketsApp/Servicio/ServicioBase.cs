using Microsoft.EntityFrameworkCore;

namespace TicketsApp.Servicio;

// Clase base para servicio
public class ServicioBase<S>
{
    protected readonly IDbContextFactory<TicketsDbContext> fabrica;
    private readonly ILogger<S> _log;

    public ServicioBase(IDbContextFactory<TicketsDbContext> fabrica, ILogger<S> log)
    {
        this.fabrica = fabrica;
        _log = log;
    }

    public async Task<MensajeDeRespuesta<T>> EjecutarServicio<T>(
        Func<Task<T>> accion,
        string mensajeDeExito,
        string mensajeDeError,
        Func<T, string>? mensajeDelegado = null)
    {
        try
        {
            var respuesta = await accion();
            var mensaje = mensajeDelegado?.Invoke(respuesta) ?? mensajeDeExito;
            _log.LogInformation(mensaje);

            return new MensajeDeRespuesta<T>
            {
                exitoso = true,
                data = respuesta,
                mensaje = mensaje
            };
        }
        catch (Exception ex)
        {
            _log.LogError(ex, mensajeDeError);

            return new MensajeDeRespuesta<T>
            {
                exitoso = false,
                data = default,
                mensaje = mensajeDeError
            };
        }
    }
}