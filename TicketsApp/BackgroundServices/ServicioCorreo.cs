using DocumentFormat.OpenXml.Drawing.Diagrams;

public class ServicioDeFondoCorreo : BackgroundService
{
    private readonly EmailQueue _cola;
    private readonly IServiceScopeFactory _fabricaScope;
    private readonly ILogger<ServicioDeFondoCorreo> _logger;

    public ServicioDeFondoCorreo(EmailQueue cola, IServiceScopeFactory fabricaScope, ILogger<ServicioDeFondoCorreo> logger)
    {
        _cola = cola;
        _fabricaScope = fabricaScope;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var correo = await _cola.DequeueAsync(token);

                using var scope = _fabricaScope.CreateScope();

                var remitente = scope.ServiceProvider.GetRequiredService<ServicioCorreo>();

                await remitente.MandarEmailAsync(correo.Para, correo.Asunto, correo.html);
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                Console.WriteLine($"Servicio de correo deteniendose...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Mandar correo");
            }
        }
    }
}