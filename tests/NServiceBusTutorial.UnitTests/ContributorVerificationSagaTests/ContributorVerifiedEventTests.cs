using FluentAssertions;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using NServiceBusTutorial.Saga;
using Xunit;

namespace NServiceBusTutorial.UnitTests.ContributorVerificationSagaTests;
public class ContributorVerifiedEventTests
{
  [Fact]
  public async Task ShouldMarkSagaAsCompleted()
  {
    var message = new ContributorVerifiedEvent();
    var saga = new ContributorVerificationSaga();
    var context = new TestableMessageHandlerContext();

    await saga.Handle(message, context);

    saga.Completed.Should().BeTrue();
  }
}
