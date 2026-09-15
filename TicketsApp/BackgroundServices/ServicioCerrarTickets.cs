namespace TicketsApp.Servicios;

public class ServicoCerrarTickets : BackgroundService
{
    private readonly IDbContextFactory<TicketsDbContext> _fabrica;

    public ServicoCerrarTickets(IDbContextFactory<TicketsDbContext> fabrica)
    {
        _fabrica = fabrica;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var contexto = await _fabrica.CreateDbContextAsync();

            var tickets = await contexto.Tickets.Where(t => t.FkEstatus == 3 && t.FechaCierreParcial < DateTime.Now.AddHours(-24)).ToListAsync();

            foreach (var x in tickets)
            {
                x.FkEstatus = 4;
                x.CerradoPorUsuario = false;
                x.FechaCierre = DateTime.Now;
            }

            await contexto.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        }
    }
}