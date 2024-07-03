using Ardalis.SharedKernel;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Specifications;
using NServiceBusTutorial.Core.Interfaces;

namespace NServiceBusTutorial.Worker.Contributors;

public class VerifyContributorCommandHandler
  : IHandleMessages<VerifyContributorCommand>
{
  private readonly INotificationService _notificationService;
  private readonly IRepository<Contributor> _repository;

  public VerifyContributorCommandHandler(INotificationService notificationService, IRepository<Contributor> repository)
  {
    _notificationService = notificationService;
    _repository = repository;
  }

  public async Task Handle(VerifyContributorCommand message, IMessageHandlerContext context)
  {
    var contributor = await _repository.SingleOrDefaultAsync(new ContributorByIdSpec(message.ContributorId), context.CancellationToken);
    if (contributor is null)
    {
      throw new InvalidOperationException($"Contributor with ID {message.ContributorId} not found.");
    }
    string textMessage = "Please verify your phone number by replying with the code: 1234";
    await _notificationService.SendSmsAsync(contributor.PhoneNumber!.Number, textMessage);
  }
}
