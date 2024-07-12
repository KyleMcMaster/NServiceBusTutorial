using Ardalis.SharedKernel;
using FluentAssertions;
using FluentAssertions.Execution;
using NServiceBus.Testing;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using NServiceBusTutorial.Worker.Contributors;
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
    var phoneNumber = new PhoneNumber(string.Empty, message.PhoneNumber, string.Empty);
    var contributor = new Contributor(message.Name, phoneNumber, ContributorStatus.NotSet, VerificationStatus.Pending)
    {
      Id = 1
    };
    var repository = Substitute.For<IRepository<Contributor>>();
    repository
      .AddAsync(Arg.Any<Contributor>(), Arg.Any<CancellationToken>())
      .Returns(contributor);
    var context = new TestableMessageHandlerContext();
    var handler = new ContributorCreateCommandHandler(repository);

    await handler.Handle(message, context);

    using var assertionScope = new AssertionScope();
    var publishedMessage = context.PublishedMessages.Single();
    publishedMessage.Message.Should().BeOfType<ContributorCreatedEvent>();
    publishedMessage.Message.As<ContributorCreatedEvent>().ContributorId.Should().Be(contributor.Id);
    publishedMessage.Message.As<ContributorCreatedEvent>().Name.Should().Be(message.Name);
    publishedMessage.Message.As<ContributorCreatedEvent>().Status.Should().Be(ContributorStatus.NotSet.ToString());
  }
}
