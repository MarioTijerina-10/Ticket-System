using System.Threading.Channels;
using MailKit;

public class EmailQueue
{
    private readonly Channel<EmailRequest> cola;

    public EmailQueue()
    {
        cola = Channel.CreateUnbounded<EmailRequest>();
    }

    public async ValueTask QueueEmailAsync(EmailRequest solicitud)
    {
        await cola.Writer.WriteAsync(solicitud);
    }

    public async ValueTask<EmailRequest> DequeueAsync(CancellationToken token)
    {
        return await cola.Reader.ReadAsync(token);
    }
}