using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NServiceBusTutorial.UseCases.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
