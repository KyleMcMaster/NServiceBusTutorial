namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;
public class UnverifyContributorCommand : ICommand
{
  public int ContributorId { get; set; }
}
