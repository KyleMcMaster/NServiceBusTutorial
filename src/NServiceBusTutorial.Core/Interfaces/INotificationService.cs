namespace NServiceBusTutorial.Core.Interfaces;

public interface INotificationService
{
  Task SendSmsAsync(string phoneNumber, string text);
}
