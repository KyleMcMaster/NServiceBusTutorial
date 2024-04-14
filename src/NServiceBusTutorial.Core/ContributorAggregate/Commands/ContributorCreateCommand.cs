namespace NServiceBusTutorial.Core.ContributorAggregate.Commands;

public class ContributorCreateCommand : ICommand
{
  public string Name { get; init; } = string.Empty;
  public string PhoneNumber { get; init; } = string.Empty;
}
