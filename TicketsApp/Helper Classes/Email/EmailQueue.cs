using System.Threading.Channels;
using MailKit;

public class EmailQueue
{
    private readonly Channel<EmailRequest> canal;

    public EmailQueue()
    {
        canal = Channel.CreateUnbounded<EmailRequest>();
    }

    public async ValueTask QueueEmailAsync(EmailRequest solicitud)
    {
        await canal.Writer.WriteAsync(solicitud);
    }

    public async ValueTask<EmailRequest> DequeueAsync(CancellationToken token)
    {
        return await canal.Reader.ReadAsync(token);
    }
}