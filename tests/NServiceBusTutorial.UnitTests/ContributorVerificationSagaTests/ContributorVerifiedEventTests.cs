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
    var saga = new TestableSaga<ContributorVerificationSaga, ContributorVerificationSagaData>();
    var context = new TestableMessageHandlerContext();

    var result = await saga.Handle(message, context);

    result.Completed.Should().BeTrue();
  }
}
