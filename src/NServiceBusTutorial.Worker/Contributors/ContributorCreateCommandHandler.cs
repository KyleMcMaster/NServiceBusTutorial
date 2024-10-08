﻿using Ardalis.SharedKernel;
using NServiceBusTutorial.Core.ContributorAggregate;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Worker.Contributors;

public class ContributorCreateCommandHandler(IRepository<Contributor> repository) 
  : IHandleMessages<ContributorCreateCommand>
{
  private readonly IRepository<Contributor> _repository = repository;
  public async Task Handle(ContributorCreateCommand message, IMessageHandlerContext context)
  {
    var phoneNumber = new PhoneNumber(string.Empty, message.PhoneNumber, string.Empty);
    var contributor = new Contributor(message.Name, phoneNumber, ContributorStatus.NotSet, VerificationStatus.Pending);
    var created = await _repository.AddAsync(contributor, context.CancellationToken);

    await context.Publish(new ContributorCreatedEvent
    {
      ContributorId = created.Id,
      Name = contributor.Name,
      Status = contributor.Status.ToString()
    });
  }
}
