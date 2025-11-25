namespace API.Services
{
    public interface INotificationService
    {
        Task SendMessageAsync(string message);
    }
}
