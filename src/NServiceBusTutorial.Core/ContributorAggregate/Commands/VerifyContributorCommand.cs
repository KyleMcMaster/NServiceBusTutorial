namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;

public class VerifyContributorCommand : ICommand
{
  public int ContributorId { get; set; }
}
