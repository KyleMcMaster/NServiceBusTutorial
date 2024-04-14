using Ardalis.SharedKernel;
using FluentAssertions;
using FluentAssertions.Execution;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using NServiceBusTutorial.Worker;
using NSubstitute;
using Xunit;

namespace NServiceBusTutorial.UnitTests.ContributorCreateCommandHandlerTests;

public class Handle
{
  [Fact]
  public async Task ShouldPublishContributorCreatedEvent()
  {
    var message = new ContributorCreateCommand
    {
      Name = "Test Contributor",
      PhoneNumber = "123-456-7890"
    };
    var context = new TestableMessageHandlerContext();
    var repository = Substitute.For<IRepository<Contributor>>();
    var handler = new ContributorCreateCommandHandler(repository);

    await handler.Handle(message, context);

    using var assertionScope = new AssertionScope();
    var publishedMessage = context.PublishedMessages.Single();
    publishedMessage.Should().BeOfType<ContributorCreatedEvent>();
    publishedMessage.As<ContributorCreatedEvent>().Name.Should().Be(message.Name);
    publishedMessage.As<ContributorCreatedEvent>().Status.Should().Be(ContributorStatus.NotSet.Name);
  }
}
