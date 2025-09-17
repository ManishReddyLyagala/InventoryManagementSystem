namespace InventoryManagement_Backend.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string body, List<string> recipients);
    }
}
