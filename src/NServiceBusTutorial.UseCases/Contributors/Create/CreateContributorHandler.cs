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


    var message = new Core.ContributorAggregate.Commands.CreateContributorCommand
    {
      Name = createdItem.Name,
      Status = createdItem.Status.ToString()
    };
    // When referencing "Getting started with NServiceBus" use:
    // await _messageSession.Send(message, cancellationToken);
    await _messageSession.Publish(message, cancellationToken);

    return createdItem.Id;
  }
}
