public interface IEmailService
{
    Task MandarEmailAsync(
        string para,
        string asunto,
        string htmlBody);
}