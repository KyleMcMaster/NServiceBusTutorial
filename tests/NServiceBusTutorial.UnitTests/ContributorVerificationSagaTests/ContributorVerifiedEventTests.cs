using FluentAssertions;
using FluentAssertions.Execution;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using NServiceBusTutorial.Saga;
using Xunit;

namespace NServiceBusTutorial.UnitTests.ContributorVerificationSagaTests;
public class ContributorVerifiedEventTests
{
  [Fact]
  public async void ShouldMarkSagaAsCompleted()
  {
    var message = new ContributorVerifiedEvent();
    var saga = new TestableSaga<ContributorVerificationSaga, ContributorVerificationSagaData>();
    var context = new TestableMessageHandlerContext();

    var result = await saga.Handle(message, context);

    result.Completed.Should().BeTrue();
  }

  [Fact]
  public async void ShouldInitializeSagaAndMarkAsCompleted()
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
    result.SagaDataSnapshot.VerificationStatus.Should().Be(VerificationStatus.Pending);

    result = await saga.Handle(message, context);
    result.Completed.Should().BeTrue();
    var timeoutMessage = result.FindTimeoutMessage<ContributorVerificationSagaTimeout>();
    timeoutMessage.Should().NotBeNull();
  }
}
