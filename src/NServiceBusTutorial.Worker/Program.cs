using System.Security.Cryptography.Xml;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBusTutorial.Core.ContributorAggregate.Commands;
using NServiceBusTutorial.Core.Interfaces;
using NServiceBusTutorial.Infrastructure.Data;
using NServiceBusTutorial.Infrastructure.Notifications;
using NServiceBusTutorial.Worker.Contributors;

var builder = Host.CreateDefaultBuilder();

builder.UseConsoleLifetime();

builder.ConfigureServices((hostContext, services) =>
{
  string? connectionString = hostContext.Configuration.GetConnectionString("SqliteConnection");
  Guard.Against.Null(connectionString);
  services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

  services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
  services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

  var mediatRAssemblies =
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([typeof(ContributorCreateCommandHandler).Assembly]));
  services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
  services.AddScoped<INotificationService, NoOpNotificationService>();
});

builder.UseNServiceBus(context => 
{
  var endpointConfiguration = new EndpointConfiguration("contributors-worker");
  var transport = endpointConfiguration.UseTransport<LearningTransport>();
  transport.Routing().RouteToEndpoint(
    typeof(StartContributorVerificationCommand),
    "contributors-saga");

  var recoverability = endpointConfiguration.Recoverability();
  recoverability.Immediate(c => c.NumberOfRetries(0));
  recoverability.Delayed(c => c.NumberOfRetries(0));

  return endpointConfiguration;
});

var host = builder.Build();
host.Run();
