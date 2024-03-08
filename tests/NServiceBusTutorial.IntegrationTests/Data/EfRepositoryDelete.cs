using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.TestUtilities.Builders;
using Xunit;

namespace NServiceBusTutorial.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // add a Contributor
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var Contributor = new ContributorBuilder()
      .WithTestValues()
      .WithName(initialName)
      .Build();
    await repository.AddAsync(Contributor);

    // delete the item
    await repository.DeleteAsync(Contributor);

    // verify it's no longer there
    Assert.DoesNotContain(await repository.ListAsync(),
        Contributor => Contributor.Name == initialName);
  }
}
