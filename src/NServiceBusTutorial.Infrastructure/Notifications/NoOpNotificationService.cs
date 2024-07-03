using NServiceBusTutorial.Core.Interfaces;

namespace NServiceBusTutorial.Infrastructure.Notifications;

public class NoOpNotificationService : INotificationService
{
  public Task SendSmsAsync(string phoneNumber, string text)
  {
    // This is a placeholder for a real SMS service
    return Task.CompletedTask;
  }
}
