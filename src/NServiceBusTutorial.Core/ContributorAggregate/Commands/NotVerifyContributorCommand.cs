namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;
public class NotVerifyContributorCommand : ICommand
{
  public int ContributorId { get; set; }
}
