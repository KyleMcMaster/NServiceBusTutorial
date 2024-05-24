using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;

namespace NServiceBusTutorial.Saga;
public class ContributorVerificationSaga : Saga<ContributorVerificationSagaData>,
  IAmStartedByMessages<StartContributorVerificationCommand>
{
  protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ContributorVerificationSagaData> mapper)
  {
    mapper.MapSaga(data => data.ContributorId)
      .ToMessage<StartContributorVerificationCommand>(message => message.ContributorId);
  }

  public Task Handle(StartContributorVerificationCommand message, IMessageHandlerContext context)
  {
    throw new NotImplementedException();
  }
}

public class ContributorVerificationSagaData : ContainSagaData
{
  public Guid ContributorId { get; set; }
}
