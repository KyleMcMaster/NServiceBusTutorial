using NServiceBusTutorial.Core.ContributorAggregate;
using Xunit;

namespace NServiceBusTutorial.UnitTests.Core.ContributorAggregate;

public class ContributorConstructor
{
  private readonly string _testName = "test name";

  [Fact]
  public void InitializesName()
  {
    var testContributor = new Contributor(_testName, new(string.Empty, "123-456-7890", string.Empty), ContributorStatus.NotSet);

    Assert.Equal(_testName, testContributor.Name);
  }
}
