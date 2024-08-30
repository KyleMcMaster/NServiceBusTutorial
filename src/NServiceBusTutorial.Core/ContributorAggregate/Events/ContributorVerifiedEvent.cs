namespace NServiceBusTutorial.Core.ContributorAggregate.Events;
public class ContributorVerifiedEvent : IEvent
{
  public int ContributorId { get; set; }
}
