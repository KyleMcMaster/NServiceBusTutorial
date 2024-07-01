using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Worker;

public class ContributorCreatedEventHandler(ILogger<ContributorCreatedEventHandler> logger) 
  : IHandleMessages<ContributorCreatedEvent>
{
  private readonly ILogger<ContributorCreatedEventHandler> _logger = logger;

  public async Task Handle(ContributorCreatedEvent message, IMessageHandlerContext context)
  {
    _logger.LogInformation("Received {EventName} for {ContributorId}",
      nameof(ContributorCreatedEvent),
      message.ContributorId);

    await context.Send(new StartContributorVerificationCommand
    {
      ContributorId = message.ContributorId
    });
  }
}
