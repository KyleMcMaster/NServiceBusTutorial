using Ardalis.Result;
using NServiceBusTutorial.UseCases.Contributors.Get;
using NServiceBusTutorial.UseCases.Contributors.Update;
using FastEndpoints;
using MediatR;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Events;

namespace NServiceBusTutorial.Web.Contributors;

/// <summary>
/// Verify an existing Contributor.
/// </summary>
/// <remarks>
/// Verify an existing Contributor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Verify(IMessageSession _messageSession)
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
    await _messageSession.Publish(new ContributorVerifiedEvent { ContributorId = request.ContributorId }, cancellationToken: cancellationToken);
  }
}
