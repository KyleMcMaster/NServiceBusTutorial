using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Saga;

public class ContributorVerificationSaga : Saga<ContributorVerificationSagaData>,
  IAmStartedByMessages<StartContributorVerificationCommand>,
  IHandleMessages<ContributorVerifiedEvent>,
  IHandleTimeouts<ContributorVerificationSagaTimeout>
{
  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ContributorVerificationSagaData> mapper)
  {
    mapper.MapSaga(data => data.ContributorId)
      .ToMessage<StartContributorVerificationCommand>(message => message.ContributorId)
      .ToMessage<ContributorVerifiedEvent>(message => message.ContributorId);
  }

  public async Task Handle(StartContributorVerificationCommand message, IMessageHandlerContext context)
  {
    var verifyContributorCommand = new VerifyContributorCommand { ContributorId = message.ContributorId };
    await context.Send(verifyContributorCommand);
    var timeout = new ContributorVerificationSagaTimeout { ContributorId = message.ContributorId };
    await RequestTimeout(context, DateTime.UtcNow.AddHours(24), timeout);
  }

  public Task Handle(ContributorVerifiedEvent message, IMessageHandlerContext context)
  {
    MarkAsComplete();
    return Task.CompletedTask;
  }

  public async Task Timeout(ContributorVerificationSagaTimeout state, IMessageHandlerContext context)
  {
    await context.Send(new NotVerifyContributorCommand { ContributorId = state.ContributorId });
    MarkAsComplete();
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
