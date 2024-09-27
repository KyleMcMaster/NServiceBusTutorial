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
    int expectedContributorId = 1;
    var startMessage = new StartContributorVerificationCommand()
    {
      ContributorId = expectedContributorId
    };
    var message = new ContributorVerifiedEvent()
    {
      ContributorId = expectedContributorId
    };
    var saga = new TestableSaga<ContributorVerificationSaga, ContributorVerificationSagaData>();
    var context = new TestableMessageHandlerContext();

    var result = await saga.Handle(startMessage, context);

    using var assertionScope = new AssertionScope();
    result.SagaDataSnapshot.ContributorId.Should().Be(expectedContributorId);

    result = await saga.Handle(message, context);
    result.Completed.Should().BeTrue();
    var timeoutMessage = result.FindTimeoutMessage<ContributorVerificationSagaTimeout>();
    timeoutMessage.Should().NotBeNull();
  }
}
