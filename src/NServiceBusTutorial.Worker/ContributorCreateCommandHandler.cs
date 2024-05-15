using Ardalis.SharedKernel;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Worker;

public class ContributorCreateCommandHandler(IRepository<Contributor> _repository) 
  : IHandleMessages<ContributorCreateCommand>
{
  public async Task Handle(ContributorCreateCommand message, IMessageHandlerContext context)
  {
    var phoneNumber = new PhoneNumber(string.Empty, message.PhoneNumber, string.Empty);
    var contributor = new Contributor(message.Name, phoneNumber, ContributorStatus.NotSet);
    var created = await _repository.AddAsync(contributor, context.CancellationToken);

    await context.Publish(new ContributorCreatedEvent
    {
      ContributorId = created.Id,
      Name = contributor.Name,
      Status = contributor.Status.ToString()
    });
  }
}
