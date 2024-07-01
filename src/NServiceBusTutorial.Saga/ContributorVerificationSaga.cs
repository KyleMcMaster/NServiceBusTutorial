using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Saga;

public class ContributorVerificationSaga : Saga<ContributorVerificationSagaData>,
  IAmStartedByMessages<StartContributorVerificationCommand>,
  IHandleMessages<ContributorConfirmedVerificationEvent>,
  IHandleTimeouts<ContributorVerificationSagaTimeout>
{
  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ContributorVerificationSagaData> mapper)
  {
    mapper.MapSaga(data => data.ContributorId)
      .ToMessage<StartContributorVerificationCommand>(message => message.ContributorId);
  }

  public async Task Handle(StartContributorVerificationCommand message, IMessageHandlerContext context)
  {
    var verifyContributorCommand = new VerifyContributorCommand { ContributorId = message.ContributorId };
    await context.Send(verifyContributorCommand);
    var timeout = new ContributorVerificationSagaTimeout { ContributorId = message.ContributorId };
    await RequestTimeout(context, DateTime.UtcNow.AddHours(24), timeout);
  }

  public Task Handle(ContributorConfirmedVerificationEvent message, IMessageHandlerContext context)
  {
    MarkAsComplete();
    return Task.CompletedTask;
  }

  public Task Timeout(ContributorVerificationSagaTimeout state, IMessageHandlerContext context)
  {
    MarkAsComplete();
    // TODO: Send command to mark contributor as unverified or try again?
    throw new NotImplementedException();
  }
}

public class ContributorVerificationSagaData : ContainSagaData
{
  public int ContributorId { get; set; }
}

public class ContributorVerificationSagaTimeout
{
  public int ContributorId { get; set; }
}
