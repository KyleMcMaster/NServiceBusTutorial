using Ardalis.SharedKernel;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Specifications;
using NServiceBusTutorial.Core.Interfaces;

namespace NServiceBusTutorial.Worker.Contributors;

public class VerifyContributorCommandHandler(INotificationService notificationService, IRepository<Contributor> repository)
    : IHandleMessages<VerifyContributorCommand>
{
  private readonly INotificationService _notificationService = notificationService;
  private readonly IRepository<Contributor> _repository = repository;
  public async Task Handle(VerifyContributorCommand message, IMessageHandlerContext context)
  {
    var contributor = await _repository.SingleOrDefaultAsync(new ContributorByIdSpec(message.ContributorId), context.CancellationToken);
    if (contributor is null)
    {
      throw new InvalidOperationException($"Contributor with Id {message.ContributorId} not found.");
    }
    string textMessage = "Welcome contributor! Please verify your phone number here: https://NimblePros.com/verify/123";
    await _notificationService.SendSmsAsync(contributor.PhoneNumber!.Number, textMessage);
  }
}
