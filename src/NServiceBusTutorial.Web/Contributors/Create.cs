﻿using NServiceBusTutorial.UseCases.Contributors.Create;
using FastEndpoints;
using MediatR;

namespace NServiceBusTutorial.Web.Contributors;

/// <summary>
/// Create a new Contributor
/// </summary>
/// <remarks>
/// Creates a new Contributor given a name.
/// </remarks>
public class Create(IMediator _mediator) : Endpoint<CreateContributorRequest, CreateContributorResponse>
{
  public override void Configure()
  {
    Post(CreateContributorRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Create a new Contributor.";
      //s.Description = "Create a new Contributor. A valid name is required.";
      s.ExampleRequest = new CreateContributorRequest { Name = "Contributor Name" };
    });
  }

  public override async Task HandleAsync(
    CreateContributorRequest request,
    CancellationToken cancellationToken)
  {
    var command = new CreateContributorCommand(request.Name, request.PhoneNumber);
    var result = await _mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      var response = new CreateContributorResponse(id: 0, name: request.Name); // TODO: remove Id from response
      await SendAsync(response, statusCode: 202, cancellationToken);
    }
    // TODO: Handle other cases as necessary
  }
}
