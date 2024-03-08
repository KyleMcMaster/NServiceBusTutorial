namespace NServiceBusTutorial.Core.ContributorAggregate.Events;

public class ContributorCreatedEvent : IMessage
{
  public int ContributorId { get; init; }
  public string Name { get; init; } = string.Empty;
  public string Status { get; init; } = string.Empty;
}
