namespace API.Services
{
    public class TelegramNotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _botToken;
        private readonly string _chatId;
        private readonly ILogger<TelegramNotificationService> _logger;

        public TelegramNotificationService(IConfiguration configuration, ILogger<TelegramNotificationService> logger)
        {
            _httpClient = new HttpClient();
            _botToken = configuration["Telegram:BotToken"] ?? "";
            _chatId = configuration["Telegram:ChatId"] ?? "";
            _logger = logger;
        }

        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrEmpty(_botToken) || string.IsNullOrEmpty(_chatId))
            {
                _logger.LogWarning("Telegram BotToken or ChatId is missing.");
                return;
            }

            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={_chatId}&text={Uri.EscapeDataString(message)}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to send Telegram message. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending Telegram message: {ex.Message}");
            }
        }
    }
}
