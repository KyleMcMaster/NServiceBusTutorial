using FluentAssertions;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Saga;
using Xunit;

namespace NServiceBusTutorial.UnitTests.ContributorVerificationSagaTests;

public class StartContributorVerificationCommandTests
{
  [Fact]
  public async Task ShouldSendVerifyContributorCommand()
  {
    var message = new StartContributorVerificationCommand();
    var saga = new ContributorVerificationSaga
    {
      Data = new()
    };
    var context = new TestableMessageHandlerContext();

    await saga.Handle(message, context);

    var sentMessage = context.FindSentMessage<VerifyContributorCommand>();
    sentMessage.Should().NotBeNull();
  }
}
