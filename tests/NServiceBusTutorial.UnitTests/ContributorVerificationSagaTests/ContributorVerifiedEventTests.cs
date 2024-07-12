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
    var saga = new ContributorVerificationSaga();
    var context = new TestableMessageHandlerContext();

    await saga.Handle(message, context);

    saga.Completed.Should().BeTrue();
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
    var saga = new ContributorVerificationSaga()
    {
      Data = new()
    };
    var context = new TestableMessageHandlerContext();

    await saga.Handle(startMessage, context);
    await saga.Handle(message, context);

    using var assertionScope = new AssertionScope();
    saga.Data.Should().NotBeNull();
    saga.Data.VerificationStatus.Should().Be(VerificationStatus.Pending);
    saga.Completed.Should().BeTrue();
    context.TimeoutMessages.Should().ContainSingle();
  }
}
