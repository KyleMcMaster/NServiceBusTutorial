using FastEndpoints;
using NServiceBusTutorial.Core.ContributorAggregate.Events;
using Ardalis.SharedKernel;
using NServiceBusTutorial.Core.ContributorAggregate;

namespace NServiceBusTutorial.Web.Contributors;

/// <summary>
/// Verify an existing Contributor.
/// </summary>
/// <remarks>
/// Verify an existing Contributor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Verify(IMessageSession _messageSession, IRepository<Contributor> _repository)
  : Endpoint<UpdateContributorRequest, UpdateContributorResponse>
{
  public override void Configure()
  {
    Put(VerifyContributorRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(
    UpdateContributorRequest request,
    CancellationToken cancellationToken)
  {
    var contributor = await _repository.GetByIdAsync(request.ContributorId, cancellationToken);
    
    if (contributor is null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    contributor.Verify();
    await _repository.SaveChangesAsync(cancellationToken);
    await _messageSession.Publish(new ContributorVerifiedEvent { ContributorId = request.ContributorId }, cancellationToken: cancellationToken);
  }
}
