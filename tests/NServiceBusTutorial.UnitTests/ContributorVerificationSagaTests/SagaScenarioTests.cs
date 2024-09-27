using FluentAssertions;
using FluentAssertions.Execution;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using NServiceBusTutorial.Saga;
using Xunit;

namespace NServiceBusTutorial.UnitTests.ContributorVerificationSagaTests;

public class SagaScenarioTests
{
  [Fact]
  public async Task ShouldInitializeSagaAndMarkAsCompleted()
  {
    int expectedContributorId = 4680;
    var startCommand = new StartContributorVerificationCommand
    {
      ContributorId = expectedContributorId
    };
    var verifiedEvent = new ContributorVerifiedEvent
    {
      ContributorId = expectedContributorId
    };
    var saga = new TestableSaga<ContributorVerificationSaga, ContributorVerificationSagaData>();
    var context = new TestableMessageHandlerContext();

    var startResult = await saga.Handle(startCommand, context);
    var completeResult = await saga.Handle(verifiedEvent, context);

    using var assertionScope = new AssertionScope();
    startResult.SagaDataSnapshot.ContributorId.Should().Be(expectedContributorId);
    completeResult.Completed.Should().BeTrue();
  }

  [Fact]
  public async Task ShouldTimeoutWhenTimeAdvancesOverLimit()
  {
    int expectedContributorId = 4680;
    var startCommand = new StartContributorVerificationCommand
    {
      ContributorId = expectedContributorId
    };
    var saga = new TestableSaga<ContributorVerificationSaga, ContributorVerificationSagaData>();
    var context = new TestableMessageHandlerContext();

    var startResult = await saga.Handle(startCommand, context);
    var timeouts = await saga.AdvanceTime(TimeSpan.FromHours(25));

    using var assertionScope = new AssertionScope();
    startResult.SagaDataSnapshot.ContributorId.Should().Be(expectedContributorId);
    var timeoutResult = timeouts.Single();
    timeoutResult.FindSentMessage<NotVerifyContributorCommand>().Should().NotBeNull();
    timeoutResult.Completed.Should().BeTrue();
  }
}
