// public class NotificationService
// {
//     private readonly IDbContextFactory<TicketsDbContext> _factory;
//     public event Func<string, Notificacion, Task>? OnNotification;

//     public NotificationService(IDbContextFactory<TicketsDbContext> factory) => _factory = factory;

//     public async Task NotifyAsync(string userId, string mensaje, int? ticketId = null)
//     {
//         await using var db = await _factory.CreateDbContextAsync();
//         var n = new Notificacion { UsuarioId = userId, Mensaje = mensaje, TicketId = ticketId };
//         db.Notificaciones.Add(n);
//         await db.SaveChangesAsync();

//         if (OnNotification is not null)
//             await OnNotification.Invoke(userId, n);
//     }

//     public async Task<List<Notificacion>> GetUnreadAsync(string userId)
//     {
//         await using var db = await _factory.CreateDbContextAsync();
//         return await db.Notificaciones
//             .Where(n => n.UsuarioId == userId && !n.Leida)
//             .OrderByDescending(n => n.Fecha)
//             .Take(20).ToListAsync();
//     }

//     public async Task MarkAllReadAsync(string userId)
//     {
//         await using var db = await _factory.CreateDbContextAsync();
//         await db.Notificaciones.Where(n => n.UsuarioId == userId && !n.Leida)
//             .ExecuteUpdateAsync(s => s.SetProperty(n => n.Leida, true));
//     }
// }