using NServiceBusTutorial.Core.ContributorAggregate;

namespace NServiceBusTutorial.TestUtilities.Builders;

public class ContributorBuilder
{
  private string? _name;
  private ContributorStatus? _status;

  public ContributorBuilder WithTestValues()
  {
    _status = ContributorStatus.NotSet;
    _name = "test name";

    return this;
  }

  public ContributorBuilder WithName(string name)
  {
    _name = name;
    return this;
  }

  public Contributor Build()
  {
    return new Contributor(_name!, new PhoneNumber(string.Empty, "123-456-7890", string.Empty), _status!);
  }
}
