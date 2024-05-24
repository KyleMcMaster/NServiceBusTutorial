namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;

public class StartContributorVerificationCommand : ICommand
{
  public Guid ContributorId { get; init; }
}
