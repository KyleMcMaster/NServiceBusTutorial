using Ardalis.Result;
using Ardalis.SharedKernel;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;

namespace NServiceBusTutorial.UseCases.Contributors.Create;

public class CreateContributorHandler(IMessageSession messageSession)
  : ICommandHandler<CreateContributorCommand, Result>
{
  private readonly IMessageSession _messageSession = messageSession;

  public async Task<Result> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
  {
    var message = new ContributorCreateCommand
    {
      Name = request.Name,
      PhoneNumber = request.PhoneNumber
    };

    await _messageSession.Send(message, cancellationToken);

    return Result.Success();
  }
}
