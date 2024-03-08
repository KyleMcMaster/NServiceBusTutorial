using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.TestUtilities.Builders;
using Xunit;

namespace NServiceBusTutorial.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsContributorAndSetsId()
  {
    string testContributorName = "testContributor";
    var testContributorStatus = ContributorStatus.NotSet;
    var repository = GetRepository();
    var contributor = new ContributorBuilder()
      .WithTestValues()
      .WithName(testContributorName)
      .Build();

    await repository.AddAsync(contributor);

    var newContributor = (await repository.ListAsync())
                    .FirstOrDefault();

    Assert.Equal(testContributorName, newContributor?.Name);
    Assert.Equal(testContributorStatus, newContributor?.Status);
    Assert.True(newContributor?.Id > 0);
  }
}
