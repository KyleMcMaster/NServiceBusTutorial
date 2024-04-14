using Ardalis.SharedKernel;
using FluentAssertions;
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

    context.PublishedMessages.Single().Message.Should().BeOfType<ContributorCreatedEvent>();
  }
}
