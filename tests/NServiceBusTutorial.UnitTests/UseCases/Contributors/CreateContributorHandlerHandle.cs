using Ardalis.SharedKernel;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.UseCases.Contributors.Create;
using FluentAssertions;
using NSubstitute;
using Xunit;
using NServiceBus;
using NServiceBus.Testing;

namespace NServiceBusTutorial.UnitTests.UseCases.Contributors;

public class CreateContributorHandlerHandle
{
  private readonly string _testName = "test name";
  private readonly string _testPhoneNumber = "123-456-7890";
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private readonly IMessageSession _messageSession = new TestableMessageSession();
  private CreateContributorHandler _handler;

  public CreateContributorHandlerHandle()
  {
    _handler = new CreateContributorHandler(_messageSession, _repository);
  }

  private Contributor CreateContributor()
  {
    return new Contributor(_testName, new PhoneNumber("", _testPhoneNumber, ""), ContributorStatus.NotSet);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidName()
  {
    _repository.AddAsync(Arg.Any<Contributor>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreateContributor()));
    var result = await _handler.Handle(new CreateContributorCommand(_testName, _testPhoneNumber), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
  }
}
