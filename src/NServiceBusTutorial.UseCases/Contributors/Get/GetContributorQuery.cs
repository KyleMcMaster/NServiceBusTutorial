using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NServiceBusTutorial.UseCases.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
