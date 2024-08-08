using Ardalis.SharedKernel;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;

namespace NServiceBusTutorial.Worker.Contributors;
public class UnverifyContributorCommandHandler(IRepository<Contributor> _repository) : IHandleMessages<UnverifyContributorCommand>
{
  public async Task Handle(UnverifyContributorCommand message, IMessageHandlerContext context)
  {
    var contributor = await _repository.GetByIdAsync(message.ContributorId, context.CancellationToken);

    if (contributor is null)
    {
      return;
    }

    contributor.Unverify();
    await _repository.SaveChangesAsync(context.CancellationToken);
  }
}
