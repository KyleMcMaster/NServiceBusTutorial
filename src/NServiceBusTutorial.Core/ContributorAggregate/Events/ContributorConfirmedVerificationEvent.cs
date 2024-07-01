namespace NServiceBusTutorial.Core.ContributorAggregate.Events;
public class ContributorConfirmedVerificationEvent : IEvent
{
  public int ContributorId { get; set; }
}
