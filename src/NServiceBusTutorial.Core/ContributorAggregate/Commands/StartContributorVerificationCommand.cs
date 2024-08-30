namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;

public class StartContributorVerificationCommand : ICommand
{
  public int ContributorId { get; init; }
}
