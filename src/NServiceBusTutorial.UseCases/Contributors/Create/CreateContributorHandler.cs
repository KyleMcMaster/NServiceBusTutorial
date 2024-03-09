using Ardalis.Result;
using Ardalis.SharedKernel;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.UseCases.Contributors.Create;

public class CreateContributorHandler(IMessageSession messageSession, IRepository<Contributor> repository)
  : ICommandHandler<CreateContributorCommand, Result<int>>
{
  private readonly IMessageSession _messageSession = messageSession;
  private readonly IRepository<Contributor> _repository = repository;

  public async Task<Result<int>> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
  {
    var phoneNumber = new PhoneNumber(string.Empty, request.PhoneNumber, string.Empty);
    var newContributor = new Contributor(request.Name, phoneNumber, ContributorStatus.NotSet);
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    var message = new ContributorCreatedEvent
    {
      ContributorId = createdItem.Id,
      Name = createdItem.Name,
      Status = createdItem.Status.ToString()
    };
    await _messageSession.Send(message, cancellationToken);

    return createdItem.Id;
  }
}
