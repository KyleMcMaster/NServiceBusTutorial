using Ardalis.Result;
using Ardalis.SharedKernel;

namespace NServiceBusTutorial.UseCases.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
